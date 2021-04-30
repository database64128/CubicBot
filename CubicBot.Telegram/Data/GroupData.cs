using System.Collections.Generic;

namespace CubicBot.Telegram.Data
{
    public class GroupData
    {
        public ulong MessagesProcessed { get; set; }
        public ulong CommandsHandled { get; set; }

        public Dictionary<long, UserData> Members { get; set; } = new();
    }
}
