using System;

namespace Meel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Initialize();
            var server = new Server("127.0.0.1", 13000);
            Console.WriteLine("Server started...");
            server.Listen().GetAwaiter().GetResult();
            Console.WriteLine("Server stopped");
        }

        public static void Initialize()
        {
            // Generate MetadataKeys
            // Load command factory
        }
    }
}
