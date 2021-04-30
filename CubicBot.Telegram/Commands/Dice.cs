using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public class Dice
    {
        public BotCommandWithHandler[] Commands => new BotCommandWithHandler[]
        {
            new("dice", "🎲 Dice it!", SendDiceAsync),
            new("dart", "🎯 Oh shoot!", SendDartAsync),
            new("basketball", "🏀 404 Basket Not Found", SendBasketballAsync),
            new("soccer", "⚽ It's your goal!", SendSoccerBallAsync),
            new("roll", "🎰 Feeling unlucky as hell?", SendSlotMachineAsync),
            new("bowl", "🎳 Can you knock them all down?", SendBowlingBallAsync),
        };

        private readonly Random _random;

        public Dice(Random random) => _random = random;

        private int GetCountFromArgument(string? argument = null)
        {
            if (int.TryParse(argument, out var specifiedCount) && specifiedCount is > 0 and <= 7)
                return specifiedCount;
            else
                return _random.Next(3);
        }

        public Task SendDiceAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var count = GetCountFromArgument(argument);
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(botClient.SendDiceAsync(message.Chat.Id, Emoji.Dice, disableNotification: true, cancellationToken: cancellationToken));

            return Task.WhenAll(tasks);
        }

        public Task SendDartAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var count = GetCountFromArgument(argument);
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(botClient.SendDiceAsync(message.Chat.Id, Emoji.Darts, disableNotification: true, cancellationToken: cancellationToken));

            return Task.WhenAll(tasks);
        }

        public Task SendBasketballAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var count = GetCountFromArgument(argument);
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(botClient.SendDiceAsync(message.Chat.Id, Emoji.Basketball, disableNotification: true, cancellationToken: cancellationToken));

            return Task.WhenAll(tasks);
        }

        public Task SendSoccerBallAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var count = GetCountFromArgument(argument);
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(botClient.SendDiceAsync(message.Chat.Id, Emoji.Football, disableNotification: true, cancellationToken: cancellationToken));

            return Task.WhenAll(tasks);
        }

        public Task SendSlotMachineAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var count = GetCountFromArgument(argument);
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(botClient.SendDiceAsync(message.Chat.Id, Emoji.SlotMachine, disableNotification: true, cancellationToken: cancellationToken));

            return Task.WhenAll(tasks);
        }

        public Task SendBowlingBallAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var count = GetCountFromArgument(argument);
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(botClient.SendDiceAsync(message.Chat.Id, Emoji.Bowling, disableNotification: true, cancellationToken: cancellationToken));

            return Task.WhenAll(tasks);
        }
    }
}
