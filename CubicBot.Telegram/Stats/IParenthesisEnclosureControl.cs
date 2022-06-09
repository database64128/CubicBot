namespace CubicBot.Telegram.Stats;

/// <summary>
/// Defines properties for configuring parenthesis enclosure enforcement behavior.
/// </summary>
public interface IParenthesisEnclosureControl
{
    /// <summary>
    /// Gets or sets whether to ensure private or group chat messages are properly
    /// enclosed in parentheses if any.
    /// </summary>
    public bool EnsureParenthesisEnclosure { get; set; }
}
