using System.Collections.Generic;

namespace GameServer.GameLogic
{
    public class Room
    {
        public List<Player> Players { get; } = new List<Player>();

        // 当前Painter, 游戏阶段，目标对象信息，当前轮次，分数统计等
        // 示例：
        public string CurrentPainterUID { get; set; } = "";
        public bool GameStarted { get; set; } = false;

        // 其他状态和方法（添加玩家、移除玩家、检查准备状态等）
    }
}
