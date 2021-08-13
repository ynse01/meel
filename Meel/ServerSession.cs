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
    public sealed class ServerSession
    {
        private static readonly byte[] Greeting =
            Encoding.ASCII.GetBytes("* OK Meel server ready for action\r\n");
        private static readonly byte[] StartTlsWarning = 
            Encoding.ASCII.GetBytes(" OK Begin TLS negotiation now\r\n");
        private static long nextSessionId = 0L;

        private ConnectionContext session;
        private Stream output;
        private int expectLiteralOfSize;
        private ImapCommands literalCommand;
        private ReadOnlySequence<byte> literalRequestId;
        private CommandFactory factory;

        public ServerSession(Stream output, IMailStation station)
        {
            expectLiteralOfSize = 0;
            this.output = output;
            session = new ConnectionContext(Interlocked.Increment(ref nextSessionId));
            factory = new CommandFactory(station);
            SendGreeting();
        }

        internal void ProcessLine(ReadOnlySequence<byte> line)
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
            ImapResponse response = new ImapResponse(output);
            var command = factory.GetCommand(literalCommand);
            command.ReceiveLiteral(session, literalRequestId, literal, ref response);
            response.SendToPipe();
        }

        private void ExecuteCommand(ImapCommands request, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> options)
        {
            var response = new ImapResponse(output);
            var command = factory.GetCommand(request);
            var literalSize = command.Execute(session, requestId, options, ref response);
            response.SendToPipe();
            if (literalSize > 0)
            {
                expectLiteralOfSize = literalSize;
                literalCommand = request;
                literalRequestId = requestId;
            }
        }

        private void SendGreeting()
        {
            output.Write(Greeting);
            output.Flush();
        }

        private void UpgradeToTls(ReadOnlySpan<byte> requestId)
        {
            output.Write(requestId);
            output.Write(StartTlsWarning);
            output.Flush();
            var sslStream = new SslStream(output);
            var ecdsa = ECDsa.Create();
            var request = new CertificateRequest("cn=MEEL", ecdsa, HashAlgorithmName.SHA256);
            var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
            sslStream.AuthenticateAsServer(certificate, false, false);
            output = sslStream;
        }
    }
}
