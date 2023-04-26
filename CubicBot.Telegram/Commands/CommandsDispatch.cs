using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Commands;

public sealed class CommandsDispatch : IDispatch
{
    private readonly Config _config;
    private readonly Data _data;
    private readonly string _botUsername;
    private readonly Dictionary<string, CubicBotCommand> _commandDict;

    public ReadOnlyCollection<CubicBotCommand> Commands { get; }

    public CommandsDispatch(Config config, Data data, string botUsername)
    {
        _config = config;
        _data = data;
        _botUsername = botUsername;

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
        _commandDict = commands.ToDictionary(x => x.Command);
    }

    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken = default)
    {
        var (command, argument) = ChatHelper.ParseMessageIntoCommandAndArgument(messageContext.Text, _botUsername);
        if (command is null)
        {
            return Task.CompletedTask;
        }

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

    private void CountCommandsHandled(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.CommandsHandled++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.CommandsHandled++;
    }
}
