namespace GameServer.GameLogic
{
    public class Player
    {
        public string UID { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Role { get; set; } = ""; // "Painter" or "Guesser"
        public int Score { get; set; } = 0;
        public bool Ready { get; set; } = false;
    }
}
