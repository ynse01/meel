using Meel.Commands;
using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Meel
{
    public sealed class ServerSession : IDisposable
    {
        private static readonly byte[] StartTlsWarning = 
            Encoding.ASCII.GetBytes(" OK Begin TLS negotiation now\r\n");
        private static long nextSessionId = 0L;

        private Stream stream = null;
        private IRequestResponsePlane responsePlane;
        private IIdentifyable sessionId;
        private PipeWriter writer;
        private int expectLiteralOfSize;
        private ImapCommands literalCommand;
        private ReadOnlySequence<byte> literalRequestId;

        public ServerSession(IRequestResponsePlane plane, PipeWriter writer)
        {
            responsePlane = plane;
            expectLiteralOfSize = 0;
            this.writer = writer;
            sessionId = plane.CreateSession(Interlocked.Increment(ref nextSessionId));
        }

        public void Dispose()
        {
            writer?.Complete();
            writer = null;
            stream?.Dispose();
            stream = null;
        }

        internal void HandleLine(ReadOnlySequence<byte> line)
        {
            if (expectLiteralOfSize > 0)
            {
                HandleLiteral(line);
            }
            else
            {
                HandleCommand(line);
            }
        }

        private void HandleCommand(ReadOnlySequence<byte> line)
        {
            if (!line.IsEmpty)
            {
                var reader = new SequenceReader<byte>(line);
                if (reader.TryReadTo(out ReadOnlySequence<byte> requestId, LexiConstants.Space, true))
                {
                    var command = CommandParser.Parse(reader, out ReadOnlySpan<byte> options);
                    if (command == ImapCommands.StartTls)
                    {
                        UpgradeToTls(requestId.ToArray());
                    }
                    else if (command == ImapCommands.Uid)
                    {
                        reader.Advance(4);
                        command = CommandParser.Parse(reader, out options);
                        ExecuteCommand(command, requestId, options);
                    } else
                    {
                        ExecuteCommand(command, requestId, options);
                    }
                }
                else
                {
                    ExecuteCommand(ImapCommands.Bad, line, ReadOnlySpan<byte>.Empty);
                }
            }
        }

        private void HandleLiteral(ReadOnlySequence<byte> data)
        {
            if (data.Length == expectLiteralOfSize)
            {
                if (expectLiteralOfSize == 0)
                {
                    ImapResponse response = new ImapResponse(writer);
                    responsePlane.ReceiveLiteral(literalCommand, sessionId, literalRequestId, data, ref response);
                    response.SendToPipe();
                    writer.FlushAsync();
                }
            } else
            {
                Console.WriteLine("Received too little data for literal");
            }
        }

        private void ExecuteCommand(ImapCommands command, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> options)
        {
            var response = new ImapResponse(writer);
            var literalSize = responsePlane.HandleRequest(command, sessionId, requestId, options, ref response);
            Console.WriteLine($"Command {command} returned {response.ToString()}");
            response.SendToPipe();
            writer.FlushAsync();
            if (literalSize > 0)
            {
                expectLiteralOfSize = literalSize;
                literalCommand = command;
                literalRequestId = requestId;
            }
        }

        private void UpgradeToTls(ReadOnlySpan<byte> requestId)
        {
            stream.Write(requestId);
            stream.Write(StartTlsWarning, 0, StartTlsWarning.Length);
            var sslStream = new SslStream(stream);
            var ecdsa = ECDsa.Create();
            var request = new CertificateRequest("cn=MEEL", ecdsa, HashAlgorithmName.SHA256);
            var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
            sslStream.AuthenticateAsServer(certificate, false, false);
            stream = sslStream;
        }
    }
}
