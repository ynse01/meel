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
        private ConnectionContext session;
        private PipeWriter writer;
        private int expectLiteralOfSize;
        private ImapCommands literalCommand;
        private ReadOnlySequence<byte> literalRequestId;
        private CommandFactory factory;

        public ServerSession(PipeWriter writer, IMailStation station)
        {
            expectLiteralOfSize = 0;
            this.writer = writer;
            session = new ConnectionContext(Interlocked.Increment(ref nextSessionId));
            factory = new CommandFactory(station);
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
            var literal = data.Slice(0, expectLiteralOfSize);
            ImapResponse response = new ImapResponse(writer);
            var command = factory.GetCommand(literalCommand);
            command.ReceiveLiteral(session, literalRequestId, literal, ref response);
            response.SendToPipe();
            writer.FlushAsync();
        }

        private void ExecuteCommand(ImapCommands request, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> options)
        {
            var response = new ImapResponse(writer);
            var command = factory.GetCommand(request);
            var literalSize = command.Execute(session, requestId, options, ref response);
            Console.WriteLine($"Command {command} returned {response.ToString()}");
            response.SendToPipe();
            writer.FlushAsync();
            if (literalSize > 0)
            {
                expectLiteralOfSize = literalSize;
                literalCommand = request;
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
