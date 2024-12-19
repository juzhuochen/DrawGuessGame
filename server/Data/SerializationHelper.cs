using System.IO;
using System.Runtime.Serialization;

namespace GameServer.Data
{
    public static class SerializationHelper
    {
        public static byte[] Serialize<T>(T obj)
        {
            // 空实现
            return new byte[0];
        }

        public static T? Deserialize<T>(byte[] data)
        {
            // 空实现
            return default;
        }
    }
}
