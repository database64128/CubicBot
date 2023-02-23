using System.Collections.Generic;

namespace CubicBot.Telegram.Stats;

public class GroupData : IParenthesisEnclosureControl
{
    public ulong MessagesProcessed { get; set; }
    public ulong CommandsHandled { get; set; }

    public Dictionary<long, UserData> Members { get; set; } = new();

    /// <summary>
    /// Gets the member's stats object.
    /// If the object doesn't already exist,
    /// a new instance is created and added to storage.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>The member's stats object.</returns>
    public UserData GetOrCreateUserData(long id)
    {
        if (Members.TryGetValue(id, out var userData))
        {
            return userData;
        }
        else
        {
            var newUserData = new UserData();
            Members.Add(id, newUserData);
            return newUserData;
        }
    }

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
    #endregion

    /// <summary>
    /// Gets or sets whether to ensure group chat messages are properly
    /// enclosed in parentheses if any.
    /// </summary>
    public bool EnsureParenthesisEnclosure { get; set; }
}
