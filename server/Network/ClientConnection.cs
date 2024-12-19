using System;
using System.IO;
using System.Net.Sockets;
using GameServer.Protocol;
using GameServer.GameLogic;

namespace GameServer.Network
{
    public class ClientConnection
    {
        private readonly TcpClient client;
        private StreamReader? sr;
        private StreamWriter? sw;
        private NetworkStream? ns;
        private bool running = false;

        // 可以存储玩家UID或引用Player对象的ID
        public string? Uid { get; set; }

        public ClientConnection(TcpClient client)
        {
            this.client = client;
        }

        public void Start()
        {
            ns = client.GetStream();
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns) { AutoFlush = true };
            running = true;

            ClientManager.AddClient(this);

            ReceiveLoop();
        }

        public void Stop()
        {
            running = false;
            ClientManager.RemoveClient(this);
            client.Close();
        }

        private async void ReceiveLoop()
        {
            while (running)
            {
                string? line = null;
                try
                {
                    line = await sr!.ReadLineAsync();
                }
                catch
                {
                    // 读取出错，可能客户端断开
                    break;
                }

                if (line == null)
                {
                    // 客户端断开
                    break;
                }

                // 解析消息
                var (command, args) = MessageParser.Parse(line);

                // 调用游戏逻辑控制器处理消息(示例)
                // 注：GameController为单例或全局访问
                var (responses, broadcastUserCount) = GameController.Instance.HandleCommand(this, command, args);

                // 发送回应消息（如果有）
                if (responses != null)
                {
                    foreach (var resp in responses)
                    {
                        Send(resp);
                    }
                }
                if (broadcastUserCount) {
                    GameController.Instance.BroadcastUserCount();
                }
            }

            // 客户端断开处理
            GameController.Instance.HandleClientDisconnect(this);
        }

public void Send(string message)
{
    try
    {
        if (sw != null)
        {
            sw.WriteLine(message);
            sw.Flush();
        }
    }
    catch (IOException)
    {
        // 此时说明连接已断开或出现IO问题
        // 移除该客户端连接
        Stop();
    }
    catch (ObjectDisposedException)
    {
        // 流已关闭，也应移除连接
        Stop();
    }
}


        public void SendBytes(byte[] bytes)
        {
            // 二进制数据发送
            if (ns != null)
            {
                ns.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
