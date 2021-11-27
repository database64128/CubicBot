using System.Diagnostics.CodeAnalysis;

namespace CubicBot.Telegram.Stats;

/// <summary>
/// Tracks a person's pronouns.
/// </summary>
/// <param name="Subject">they</param>
/// <param name="Object">them</param>
/// <param name="PossessiveDeterminer">their</param>
/// <param name="PossessivePronoun">theirs</param>
/// <param name="Reflexive">themselves</param>
public record Pronouns(string Subject, string Object, string PossessiveDeterminer, string PossessivePronoun, string Reflexive)
{
    /// <summary>
    /// they/them/their/theirs/themselves
    /// </summary>
    public static readonly Pronouns Neutral = new("they", "them", "their", "theirs", "themselves");

    /// <summary>
    /// he/him/his/his/himself
    /// </summary>
    public static readonly Pronouns Masculine = new("he", "him", "his", "his", "himself");

    /// <summary>
    /// she/her/her/hers/herself
    /// </summary>
    public static readonly Pronouns Feminine = new("she", "her", "her", "hers", "herself");

    /// <summary>
    /// it/it/its/its/itself
    /// </summary>
    public static readonly Pronouns Neuter = new("it", "it", "its", "its", "itself");

    /// <summary>
    /// thou/thee/thy/thine/thyself
    /// </summary>
    public static readonly Pronouns Archaic = new("thou", "thee", "thy", "thine", "thyself");

    /// <summary>
    /// Returns the pronouns in the form of
    /// they/them/their/theirs/themselves.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Subject}/{Object}/{PossessiveDeterminer}/{PossessivePronoun}/{Reflexive}";

    /// <summary>
    /// Returns the pronouns in the form of
    /// they/them.
    /// </summary>
    /// <returns></returns>
    public string ToSubjectObject() => $"{Subject}/{Object}";

    /// <summary>
    /// Returns the pronouns in the form of
    /// they/them/theirs.
    /// </summary>
    /// <returns></returns>
    public string ToSubjectObjectPossessive() => $"{Subject}/{Object}/{PossessivePronoun}";

    /// <summary>
    /// Attempts to parse the input into a <see cref="Pronouns"/> record.
    /// Accepts the following formats:
    /// 1. they
    /// 2. they/them
    /// 3. they/them/their
    /// 4. they/them/theirs
    /// 5. they/them/their/theirs/themselves
    /// Format 1-4 can only be parsed into built-in pronouns.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="pronouns">The parsed <see cref="Pronouns"/> record.</param>
    /// <returns>
    /// True if the input string is successfully parsed into a <see cref="Pronouns"/> record.
    /// Otherwise, false.
    /// </returns>
    public static bool TryParse(string input, [MaybeNullWhen(false)] out Pronouns pronouns)
    {
        pronouns = input switch
        {
            "they" or "they/them" or "they/them/their" or "they/them/theirs" or "they/them/their/theirs/themselves" => Neutral,
            "he" or "he/him" or "he/him/his" or "he/him/his/his/himself" => Masculine,
            "she" or "she/her" or "she/her/her" or "she/her/hers" or "she/her/her/hers/herself" => Feminine,
            "it" or "it/it" or "it/it/its" or "it/it/its/its/itself" => Neuter,
            "thou" or "thou/thee" or "thou/thee/thy" or "thou/thee/thine" or "thou/thee/thy/thine/thyself" => Archaic,
            _ => null,
        };
        if (pronouns is not null)
        {
            return true;
        }

        var parts = input.Split('/');
        if (parts.Length == 5)
        {
            pronouns = new(parts[0], parts[1], parts[2], parts[3], parts[4]);
            return true;
        }

        return false;
    }
}
