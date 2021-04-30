using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class BotCommandWithHandler : BotCommand
    {
        public Func<ITelegramBotClient, Message, string?, CancellationToken, Task> Handler { get; set; }

        public BotCommandWithHandler(string command, string description, Func<ITelegramBotClient, Message, string?, CancellationToken, Task> handler)
        {
            Command = command;
            Description = description;
            Handler = handler;
        }
    }
}
