using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public class CubicBotCommand : BotCommand
{
    public Func<CommandContext, CancellationToken, Task> HandlerAsync { get; set; }

    public Func<CommandContext, CancellationToken, Task>? StatsCollectorAsync { get; set; }

    public Action<CommandContext>? StatsCollector { get; set; }

    public CubicBotCommand(
        string command,
        string description,
        Func<CommandContext, CancellationToken, Task> handlerAsync,
        Func<CommandContext, CancellationToken, Task>? statsCollectorAsync = null,
        Action<CommandContext>? statsCollector = null)
    {
        Command = command;
        Description = description;
        HandlerAsync = handlerAsync;
        StatsCollectorAsync = statsCollectorAsync;
        StatsCollector = statsCollector;
    }
}
