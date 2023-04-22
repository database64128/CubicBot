namespace CubicBot.Telegram.Commands;

public sealed class CommandContext : MessageContext
{
    /// <summary>
    /// Gets the command string.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// Gets the command argument string.
    /// </summary>
    public string? Argument { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext"/> class.
    /// </summary>
    /// <param name="messageContext">The <see cref="MessageContext"/> instance.</param>
    /// <param name="command">The command string.</param>
    /// <param name="argument">The command argument string.</param>
    public CommandContext(MessageContext messageContext, string command, string? argument) : base(messageContext)
    {
        Command = command;
        Argument = argument;
    }
}
