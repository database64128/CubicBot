namespace CubicBot.Telegram.Stats;

public sealed class UserData : ChatData
{
    #region 0. Personal
    public List<Pronouns> PronounList { get; set; } = [];
    public Pronouns? DefaultPronouns { get; set; }
    public Pronouns? PreferredPronouns { get; set; }
    #endregion

    #region 1. Common
    public ulong ApologiesSent { get; set; }
    public ulong ApologiesReceived { get; set; }
    public ulong ChantsUsed { get; set; }
    public ulong DrinksTaken { get; set; }
    public ulong DrankByOthers { get; set; }
    public ulong MesUsed { get; set; }
    public ulong ThankYousSent { get; set; }
    public ulong ThankYousReceived { get; set; }
    public ulong ThanksSaid { get; set; }
    public ulong VaccinationShotsReceived { get; set; }
    #endregion

    #region 3. Consent Not Needed
    public ulong MealsCooked { get; set; }
    public ulong CookedByOthers { get; set; }
    public ulong PersonsThrown { get; set; }
    public ulong ThrownByOthers { get; set; }
    public ulong PersonsCaught { get; set; }
    public ulong CaughtByOthers { get; set; }
    public ulong ForceUsed { get; set; }
    public ulong ForcedByOthers { get; set; }
    public ulong TouchesGiven { get; set; }
    public ulong TouchesReceived { get; set; }
    public ulong SexReceived { get; set; }
    #endregion

    #region 4. Not A Vegan
    public ulong FoodEaten { get; set; }
    public ulong EatenByOthers { get; set; }
    #endregion

    #region 5. Law Enforcement
    public ulong ArrestsReceived { get; set; }
    public ulong VerdictsReceived { get; set; }
    public ulong OverthrowAttemptsReceived { get; set; }
    #endregion

    #region 8. Chinese Tasks
    public ulong OkaysSaid { get; set; }
    public ulong OkaysReceived { get; set; }
    public ulong AssignmentsCreated { get; set; }
    public ulong AssignmentsReceived { get; set; }
    public ulong UnassignInitiated { get; set; }
    public ulong UnassignReceived { get; set; }
    #endregion

    #region 9. Systemd
    public ulong SystemctlCommandsUsed { get; set; }
    #endregion
}
