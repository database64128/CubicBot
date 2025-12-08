namespace CubicBot.Telegram.Stats;

public abstract class ChatData
{
    public ulong MessagesProcessed { get; set; }
    public ulong CommandsHandled { get; set; }

    #region 1. Common
    public ulong VaccinationShotsAdministered { get; set; }
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
    public ulong SexInitiated { get; set; }
    #endregion

    #region 5. Law Enforcement
    public ulong CopCallsMade { get; set; }
    public ulong ArrestsMade { get; set; }
    public ulong VerdictsGiven { get; set; }
    public ulong OverthrowAttempts { get; set; }
    #endregion

    #region 6. Public Services
    public ulong AmbulancesCalled { get; set; }
    public ulong FiresReported { get; set; }
    #endregion

    #region 7. Chinese
    public ulong InterrogationsInitiated { get; set; }
    public ulong InterrogatedByOthers { get; set; }
    #endregion

    public ulong GrassGrown { get; set; }

    /// <summary>
    /// Gets or sets whether to reply to messages containing
    /// unenclosed parentheses to complete the enclosure.
    /// </summary>
    public bool EnsureParenthesisEnclosure { get; set; }

    public ulong ParenthesesUnenclosed { get; set; }

    public ulong TwoTripleThreesUsed { get; set; }
}
