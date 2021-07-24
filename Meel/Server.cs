using Meel.Stations;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
            server.Start();
            Listen();
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    var station = new InMemoryStation();
                    var plane = new LocalRequestResponsePlane(station);
                    var session = new ServerSession(plane);
                    var id = Interlocked.Increment(ref lastId);
                    var thread = new Thread(session.ThreadStarter);
                    var startParameter = Tuple.Create(client, id);
                    thread.Start(startParameter);
                }
            } catch(SocketException ex)
            {
                Console.WriteLine($"Socket exception: {ex}");
                server.Stop();
            }
        }

        private async Task ListenByPipe()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    var client = await server.AcceptTcpClientAsync();
                    Console.WriteLine("Connected!");
                    var station = new InMemoryStation();
                    var plane = new LocalRequestResponsePlane(station);
                    var session = new ServerPipe(plane);
                    var id = Interlocked.Increment(ref lastId);
                    _ = session.ProcessAsync(client);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket exception: {ex}");
                server.Stop();
            }
        }

    }
}
