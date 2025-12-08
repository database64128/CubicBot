namespace CubicBot.Telegram.Commands;

/// <summary>
/// Initializes a new instance of the <see cref="CommandContext"/> class.
/// </summary>
/// <param name="messageContext">The <see cref="MessageContext"/> instance.</param>
/// <param name="command">The command string.</param>
/// <param name="argument">The command argument string.</param>
public sealed class CommandContext(MessageContext messageContext, string command, string? argument) : MessageContext(messageContext)
{
    /// <summary>
    /// Gets the command string.
    /// </summary>
    public string Command { get; } = command;

    /// <summary>
    /// Gets the command argument string.
    /// </summary>
    public string? Argument { get; } = argument;
}
