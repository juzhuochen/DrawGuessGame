namespace GameServer.Protocol
{
    public static class MessageParser
    {
        public static (string command, string[] args) Parse(string line)
        {
            var parts = line.Split(',');
            var command = parts[0];
            var args = parts.Length > 1 ? parts[1..] : new string[0];
            return (command, args);
        }
    }
}
