using System.Net;
using System.Net.Sockets;

namespace GameServer.Network
{
    public class ServerMain
    {
        private TcpListener? listener;
        private bool running = false;

        public void Start()
        {
            // 监听某个端口
            var serverAddr = IPAddress.Loopback;
            listener = new TcpListener(serverAddr, 51888);
            listener.Start();
            running = true;
            AcceptClientsAsync();
        }

        public void Stop()
        {
            running = false;
            listener?.Stop();
        }

        private async void AcceptClientsAsync()
        {
            while (running)
            {
                try
                {
                    var client = await listener!.AcceptTcpClientAsync();
                    var connection = new ClientConnection(client);
                    connection.Start();
                }
                catch
                {
                    // 当listener停止时会产生异常，这里可忽略
                    break;
                }
            }
        }
    }
}
