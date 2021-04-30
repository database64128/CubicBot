namespace CubicBot.Telegram.Data
{
    public class UserData
    {
        public ulong MessagesProcessed { get; set; }
        public ulong CommandsHandled { get; set; }

        public ulong CopCallsMade { get; set; }
        public ulong Chanting { get; set; }
        public ulong BottlesDrank { get; set; }
        public ulong FoodEaten { get; set; }
        public ulong SexHad { get; set; }
        public ulong ThankSaid { get; set; }
        public ulong ThanksSaid { get; set; }

        public ulong DicesThrown { get; set; }
        public ulong DartsThrown { get; set; }
        public ulong BasketballsThrown { get; set; }
        public ulong SoccerGoals { get; set; }
        public ulong SlotMachineRolled { get; set; }
        public ulong PinsKnocked { get; set; }

        public ulong InterrogationInitiated { get; set; }

        public ulong GrassGrown { get; set; }
    }
}
