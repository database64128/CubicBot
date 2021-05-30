using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class CubicBotCommand : BotCommand
    {
        public Func<ITelegramBotClient, Message, string?, Config, Data, CancellationToken, Task> Handler { get; set; }

        public Func<ITelegramBotClient, Message, string?, Config, Data, CancellationToken, Task>? StatsCollector { get; set; }

        public CubicBotCommand(string command, string description, Func<ITelegramBotClient, Message, string?, Config, Data, CancellationToken, Task> handler, Func<ITelegramBotClient, Message, string?, Config, Data, CancellationToken, Task>? statsCollector = null)
        {
            Command = command;
            Description = description;
            Handler = handler;
            StatsCollector = statsCollector;
        }
    }
}
