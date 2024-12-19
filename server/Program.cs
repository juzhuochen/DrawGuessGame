using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // 启动服务器主类
            var server = new Network.ServerMain();
            server.Start();

            Console.WriteLine("Server started. Press Enter to exit.");
            Console.ReadLine();

            server.Stop();
        }
    }
}
