using System.Collections.Generic;
using System.Linq;

namespace GameServer.Network
{
    public static class ClientManager
    {
        private static readonly List<ClientConnection> clients = new List<ClientConnection>();

        public static void AddClient(ClientConnection c)
        {
            lock (clients)
            {
                clients.Add(c);
            }
        }

        public static void RemoveClient(ClientConnection c)
        {
            lock (clients)
            {
                clients.Remove(c);
            }
        }

        public static List<ClientConnection> GetAllClients()
        {
            lock (clients)
            {
                return clients.ToList();
            }
        }
    }
}
