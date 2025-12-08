using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Collections.ObjectModel;

namespace CubicBot.Telegram.Commands;

public sealed partial class CommandsDispatch : IDispatch
{
    private readonly Config _config;
    private readonly string _botUsername;
    private readonly ILogger _logger;
    private readonly FrozenDictionary<string, CubicBotCommand> _commandDict;

    public ReadOnlyCollection<CubicBotCommand> Commands { get; }

    public CommandsDispatch(Config config, string botUsername, ILogger logger)
    {
        _config = config;
        _botUsername = botUsername;
        _logger = logger;

        var commands = new List<CubicBotCommand>();

        if (config.Commands.EnablePersonal)
            Personal.AddCommands(commands);

        if (config.Commands.EnableCommon)
            Common.AddCommands(commands);

        if (config.Commands.EnableDice)
            Dice.AddCommands(commands);

        if (config.Commands.EnableConsentNotNeeded)
            ConsentNotNeeded.AddCommands(commands);

        if (config.Commands.EnableNonVegan)
            NonVegan.AddCommands(commands);

        if (config.Commands.EnableLawEnforcement)
            LawEnforcement.AddCommands(commands);

        if (config.Commands.EnablePublicServices)
            PublicServices.AddCommands(commands);

        if (config.Commands.EnableChinese)
            Chinese.AddCommands(commands);

        if (config.Commands.EnableChineseTasks)
            ChineseTasks.AddCommands(commands);

        if (config.Commands.EnableSystemd)
            Systemd.AddCommands(commands);

        if (config.EnableStats)
        {
            _ = new QueryStats(config, commands);
            _ = new Controls(config, commands);
        }

        Commands = commands.AsReadOnly();
        _commandDict = commands.ToFrozenDictionary(x => x.Command);
    }

    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken = default)
    {
        var (command, argument) = ChatHelper.ParseMessageIntoCommandAndArgument(messageContext.Text, _botUsername);
        if (command is null)
        {
            return Task.CompletedTask;
        }

        LogHandlingCommand(command, argument);

        if (_commandDict.TryGetValue(command, out var botCommand))
        {
            var commandContext = new CommandContext(messageContext, command, argument);
            var handleTask = botCommand.HandlerAsync(commandContext, cancellationToken);

            if (_config.EnableStats && _config.Stats.EnableCommandStats)
            {
                CountCommandsHandled(commandContext);
                botCommand.StatsCollector?.Invoke(commandContext);
                if (botCommand.StatsCollectorAsync is not null)
                {
                    return Task.WhenAll(handleTask, botCommand.StatsCollectorAsync(commandContext, cancellationToken));
                }
            }

            return handleTask;
        }

        return Task.CompletedTask;
    }

    private static void CountCommandsHandled(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.CommandsHandled++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.CommandsHandled++;
    }

    [LoggerMessage(Level = LogLevel.Trace, Message = "Handling command {Command} with argument {Argument}")]
    private partial void LogHandlingCommand(string command, string? argument);
}
