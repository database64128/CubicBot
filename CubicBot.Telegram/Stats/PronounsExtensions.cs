using System.Linq;

namespace CubicBot.Telegram.Stats;

public static class PronounsExtensions
{
    /// <summary>
    /// Gets the pronouns to use to refer to someone in the current context.
    /// Priority: preferred pronouns in group > default pronouns > all pronouns.
    /// </summary>
    /// <param name="data">The data object.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="groupId">Optional group ID.</param>
    /// <returns>An array of pronouns that can be used to refer to someone in the current context.</returns>
    public static Pronouns[] GetPronounsToUse(this Data data, long userId, long groupId = 0L)
    {
        if (groupId != 0L)
        {
            var memberData = data.GetOrCreateGroupData(groupId).GetOrCreateUserData(userId);
            if (memberData.PreferredPronouns is not null)
            {
                return new Pronouns[] { memberData.PreferredPronouns };
            }
        }

        var userData = data.GetOrCreateUserData(userId);
        if (userData.DefaultPronouns is not null)
        {
            return new Pronouns[] { userData.DefaultPronouns };
        }

        return userData.PronounList.ToArray();
    }

    public static string GetPronounSubject(this Data data, long userId, long groupId = 0L)
    {
        var pronouns = GetPronounsToUse(data, userId, groupId);
        return pronouns.Length switch
        {
            0 => Pronouns.Neutral.Subject,
            1 => pronouns[0].Subject,
            _ => string.Join('/', pronouns.Select(x => x.Subject)),
        };
    }

    public static string GetPronounObject(this Data data, long userId, long groupId = 0L)
    {
        var pronouns = GetPronounsToUse(data, userId, groupId);
        return pronouns.Length switch
        {
            0 => Pronouns.Neutral.Object,
            1 => pronouns[0].Object,
            _ => string.Join('/', pronouns.Select(x => x.Object)),
        };
    }

    public static string GetPronounPossessiveDeterminer(this Data data, long userId, long groupId = 0L)
    {
        var pronouns = GetPronounsToUse(data, userId, groupId);
        return pronouns.Length switch
        {
            0 => Pronouns.Neutral.PossessiveDeterminer,
            1 => pronouns[0].PossessiveDeterminer,
            _ => string.Join('/', pronouns.Select(x => x.PossessiveDeterminer)),
        };
    }

    public static string GetPronounPossessivePronoun(this Data data, long userId, long groupId = 0L)
    {
        var pronouns = GetPronounsToUse(data, userId, groupId);
        return pronouns.Length switch
        {
            0 => Pronouns.Neutral.PossessivePronoun,
            1 => pronouns[0].PossessivePronoun,
            _ => string.Join('/', pronouns.Select(x => x.PossessivePronoun)),
        };
    }

    public static string GetPronounReflexive(this Data data, long userId, long groupId = 0L)
    {
        var pronouns = GetPronounsToUse(data, userId, groupId);
        return pronouns.Length switch
        {
            0 => Pronouns.Neutral.Reflexive,
            1 => pronouns[0].Reflexive,
            _ => string.Join('/', pronouns.Select(x => x.Reflexive)),
        };
    }

    public static (Pronouns[] pronouns, Pronouns? defaultPronouns, Pronouns? preferredPronouns) GetPronounsInfo(this Data data, long userId, long groupId = 0L)
    {
        Pronouns? preferredPronouns = null;

        if (groupId != 0L)
        {
            preferredPronouns = data.GetOrCreateGroupData(groupId).GetOrCreateUserData(userId).PreferredPronouns;
        }

        var userData = data.GetOrCreateUserData(userId);
        var defaultPronouns = userData.DefaultPronouns;
        var pronouns = userData.PronounList.ToArray();

        return (pronouns, defaultPronouns, preferredPronouns);
    }
}
