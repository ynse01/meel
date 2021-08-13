using Meel.Stations;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Meel
{
    public class Server
    {
        private static long lastId = -1L;

        private TcpListener server;

        public Server(string ip, int port)
        {
            var localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
        }

        public async Task Listen()
        {
            var station = new InMemoryStation();
            try
            {
                server.Start();
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    var client = await server.AcceptTcpClientAsync();
                    Console.WriteLine("Connected!");
                    var session = new ServerPipe(station);
                    var id = Interlocked.Increment(ref lastId);
                    _ = session.ProcessAsync(client);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket exception: {ex}");
                server.Stop();
            }
            station.Dispose();
        }

    }
}
