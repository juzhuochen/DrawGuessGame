// GameLogic/GameController.cs
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using GameServer.Network;
using GameServer.Protocol;

namespace GameServer.GameLogic
{
    public class GameController
    {
        private static GameController? instance;
        public static GameController Instance => instance ??= new GameController();

        private Room room;

        private int uidCounter = 1; // 简单的UID生成器，用于给新玩家分配唯一ID

        private GameController()
        {
            room = new Room();
        }

        public (List<string>? response, bool broadcastUserCount) HandleCommand(ClientConnection client, string command, string[] args)
        {
            switch (command)
            {
                case MessageType.Login:
                    return HandleLogin(client, args);
               // case MessageType.Chat:
                    //return Handle...
                // 其他指令 case 待实现...
            }
            return (null,false);
        }

        public void HandleClientDisconnect(ClientConnection client)
        {
            // 移除玩家
            if (client.Uid != null)
            {
                var p = room.Players.FirstOrDefault(x => x.UID == client.Uid);
                if (p != null)
                {
                    room.Players.Remove(p);
                    // 广播UserCount变化
                    BroadcastUserCount();
                }
            }
        }

        private (List<string>? , bool broadcastUserCount)HandleLogin(ClientConnection client, string[] args)
        {
            if (args.Length < 1)
            {
                // 参数不足
                return (new List<string> { MessageType.LoginFail }, false);
            }

            string userName = args[0];

            // 检查用户名是否已存在
            if (room.Players.Any(p => p.UserName == userName))
            {
                // 用户名重复
                return (new List<string> { MessageType.LoginFail }, false);
            }

            // 为玩家分配UID
            string uid = "P" + uidCounter++;
            var player = new Player
            {
                UID = uid,
                UserName = userName
            };

            room.Players.Add(player);
            client.Uid = uid; // 在clientconnection中记录该玩家的UID

            // 登录成功消息
            var responses = new List<string> { $"{MessageType.LoginOK},{uid}" };

            return (responses, true);
        }

        public void BroadcastUserCount()
        {
            int count = room.Players.Count;
            string msg = MessageBuilder.BuildUserCountMessage(count);
            BroadcastToAll(msg);
        }

        private void BroadcastToAll(string message)
        {
            // 在实际实现中应该有方式获取所有已连接的ClientConnection，这里简化为通过Room管理或全局列表
            // 假设有个 ClientManager 来管理所有连接中的客户端(需根据实际情况实现)
            foreach (var c in ClientManager.GetAllClients())
            {
                c.Send(message);
            }
        }
    }
}
