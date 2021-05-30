namespace CubicBot.Telegram.Stats
{
    public class UserData
    {
        public ulong MessagesProcessed { get; set; }
        public ulong CommandsHandled { get; set; }

        #region 1. Common
        public ulong ChantsUsed { get; set; }
        public ulong BottlesDrank { get; set; }
        public ulong ThanksGiven { get; set; }
        public ulong ThanksReceived { get; set; }
        public ulong ThanksSaid { get; set; }
        public ulong VaccinationShotsAdministered { get; set; }
        public ulong VaccinationShotsGot { get; set; }
        #endregion

        #region 2. Dice
        public ulong DicesThrown { get; set; }
        public ulong DartsThrown { get; set; }
        public ulong BasketballsThrown { get; set; }
        public ulong SoccerGoals { get; set; }
        public ulong SlotMachineRolled { get; set; }
        public ulong PinsKnocked { get; set; }
        #endregion

        #region 3. Consent Not Needed
        public ulong MealsCooked { get; set; }
        public ulong ForceUsed { get; set; }
        public ulong ForcedByOthers { get; set; }
        public ulong TouchesGiven { get; set; }
        public ulong TouchesReceived { get; set; }
        public ulong SexInitiated { get; set; }
        public ulong SexReceived { get; set; }
        #endregion

        #region 4. Not A Vegan
        public ulong FoodEaten { get; set; }
        public ulong EatenByOthers { get; set; }
        #endregion

        #region 5. Law Enforcement
        public ulong CopCallsMade { get; set; }
        public ulong ArrestsMade { get; set; }
        public ulong ArrestsReceived { get; set; }
        public ulong VerdictsGiven { get; set; }
        public ulong VerdictsReceived { get; set; }
        #endregion

        #region 6. Public Services
        public ulong AmbulancesCalled { get; set; }
        public ulong FiresReported { get; set; }
        #endregion

        #region 7. Chinese
        public ulong InterrogationInitiated { get; set; }
        public ulong InterrogatedByOthers { get; set; }
        #endregion

        #region 8. Chinese Tasks
        public ulong OkaysSaid { get; set; }
        public ulong OkaysReceived { get; set; }
        public ulong AssignmentsGiven { get; set; }
        public ulong AssignmentsReceived { get; set; }
        public ulong UnassignInitiated { get; set; }
        public ulong UnassignReceived { get; set; }
        #endregion

        public ulong GrassGrown { get; set; }
    }
}
