namespace CubicBot.Telegram.Stats;

public static class PronounsExtensions
{
    /// <summary>
    /// Gets the pronouns to use to refer to the message sender in the current context.
    /// Priority: preferred pronouns in chat > default pronouns > all pronouns > neutral
    /// </summary>
    /// <returns>The pronouns of the message sender in the current context.</returns>
    public static Pronouns GetPronounsToUse(this MessageContext messageContext)
        => messageContext.MemberOrUserData.PreferredPronouns
        ?? messageContext.UserData.DefaultPronouns
        ?? messageContext.UserData.PronounList.FirstOrDefault(Pronouns.Neutral);
}
