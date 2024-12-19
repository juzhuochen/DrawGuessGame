namespace GameServer.Protocol
{
    public static class MessageBuilder
    {
        public static string BuildUserCountMessage(int count)
        {
            return $"{MessageType.UserCount},{count}";
        }

        // 其他构建消息方法...
        // 例如:
        // public static string BuildLoginOKMessage(string uid) => $"{MessageType.LoginOK},{uid}";
        // public static string BuildGuessResultMessage(string uid, string result) => $"{MessageType.GuessResult},{uid},{result}";
    }
}
