using Meel.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Meel
{
    public class ServerSession
    {
        private const string LiteralEndMarker = "\r\n\r\n";
        private static readonly byte[] StartTlsWarning = 
            Encoding.ASCII.GetBytes(" OK Begin TLS negotiation now\r\n");

        private byte[] commandBuffer = new byte[2048];
        private long id;
        private Stream stream = null;
        private IRequestResponsePlane responsePlane;
        private int expectLiteralOfSize;
        private List<string> literalBuffer;
        private ImapCommands literalCommand;
        private string literalRequestId;

        public ServerSession(IRequestResponsePlane plane)
        {
            responsePlane = plane;
            expectLiteralOfSize = 0;
        }

        public ParameterizedThreadStart ThreadStarter => new ParameterizedThreadStart(HandleNetworkDevice);

        private void HandleNetworkDevice(object obj)
        {
            var tuple = (Tuple<TcpClient, long>)obj;
            var client = tuple.Item1;
            id = tuple.Item2;
            stream = client.GetStream();
            int numBytes;
            try
            {
                while ((numBytes = stream.Read(commandBuffer, 0, commandBuffer.Length)) != 0)
                {
                    var line = Encoding.ASCII.GetString(commandBuffer, 0, numBytes);
                    HandleLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.ToString());
                client.Close();
            }
        }

        internal void HandleLine(string line)
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

        private void HandleCommand(string data)
        {
            Console.WriteLine("Received {0}", data);
            var lines = data.Split("\r\n");
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                var parts = line.Split(' ', 3);
                if (parts.Length > 1)
                {
                    var requestId = parts[0];
                    var request = ParseRequest(parts[1]);
                    if (request == ImapCommands.StartTls)
                    {
                        UpgradeToTls(requestId);
                    }
                    else if (parts.Length == 3)
                    {
                        ExecuteCommand(request, requestId, parts[2]);
                    }
                    else if (parts.Length == 2)
                    {
                        ExecuteCommand(request, requestId, null);
                    }
                }
                else
                {
                    ExecuteCommand(ImapCommands.Bad, line, null);
                }
            }
        }

        private ImapCommands ParseRequest(string request)
        {
            ImapCommands result;
            if (!Enum.TryParse(request, true, out result))
            {
                result = ImapCommands.Bad;
            }
            return result;
        }

        private void HandleLiteral(string data)
        {
            var hasEndMarker = data.EndsWith(LiteralEndMarker);
            if (hasEndMarker)
            {
                literalBuffer.Add(data);
                expectLiteralOfSize--;
            }
            if (expectLiteralOfSize == 0)
            {
                responsePlane.ReceiveLiteral(literalCommand, id, literalRequestId, literalBuffer);
            }
        }

        private void ExecuteCommand(ImapCommands command, string requestId, string options)
        {
            var result = responsePlane.HandleRequest(command, id, requestId, options);
            Console.WriteLine($"Command {command} returned {result}");
            result.SendTo(stream);
            if (result.ExpectLiteralOfSize > 0)
            {
                expectLiteralOfSize = result.ExpectLiteralOfSize;
                literalBuffer = new List<string>();
                literalCommand = command;
                literalRequestId = requestId;
            }
        }

        private void UpgradeToTls(string requestId)
        {
            var ackId = Encoding.ASCII.GetBytes(requestId);
            stream.Write(ackId, 0, ackId.Length);
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
