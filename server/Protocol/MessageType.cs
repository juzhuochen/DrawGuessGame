namespace GameServer.Protocol
{
    public static class MessageType
    {
        public const string Login = "Login";
        public const string Ready = "Ready";
        public const string SubmitDrawing = "SubmitDrawing";
        public const string Guess = "Guess";
        public const string Chat = "Chat";
        public const string Exit = "Exit";

        // 服务器发给客户端的
        public const string LoginOK = "LoginOK";
        public const string LoginFail = "LoginFail";
        public const string UserCount = "UserCount";
        public const string GameStart = "GameStart";
        public const string YourRole = "YourRole";
        public const string ObjectToDraw = "ObjectToDraw";
        public const string DrawCurve = "DrawCurve";
        public const string GuessResult = "GuessResult";
        public const string RoundOver = "RoundOver";
        public const string GameOver = "GameOver";
        public const string ChatMessage = "ChatMessage";
        public const string PlayerLeft = "PlayerLeft";
    }
}
