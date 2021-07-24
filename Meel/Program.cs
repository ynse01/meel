using System;
using System.Threading;

namespace Meel
{
    class Program
    {
        private static Thread thread;

        static void Main(string[] args)
        {
            thread = new Thread(() =>
            {
                var server = new Server("127.0.0.1", 13000);
            });
            thread.Start();
            Console.WriteLine("Server started...");
        }
    }
}
