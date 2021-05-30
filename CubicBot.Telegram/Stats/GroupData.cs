using System.Collections.Generic;

namespace CubicBot.Telegram.Stats
{
    public class GroupData
    {
        public ulong MessagesProcessed { get; set; }
        public ulong CommandsHandled { get; set; }

        public Dictionary<long, UserData> Members { get; set; } = new();
    }
}
