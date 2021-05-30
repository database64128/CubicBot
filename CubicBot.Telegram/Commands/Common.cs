using CubicBot.Telegram.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public class Common
    {
        public static string[] Beverages => new string[]
        {
            "🍼", "🥛", "☕️", "🫖", "🍵", "🍶", "🍾", "🍷", "🍸", "🍹",
            "🍺", "🍻", "🥂", "🧉", "🏺", "🚰", "🧋", "🧃",
        };

        public BotCommandWithHandler[] Commands => new BotCommandWithHandler[]
        {
            new("chant", "🗣 Say it out loud!", ChantAsync),
            new("drink", "🥤 I'm thirsty!", DrinkAsync),
            new("thank", "🦃 Reply to or mention the name of the person you would like to thank.", SayThankAsync),
            new("thanks", "😊 Say thanks to me!", SayThanksAsync),
            new("vax", "💉 Gen Z also got the vax!", VaccinateAsync),
        };

        private readonly Random _random;

        public Common(Random random) => _random = random;

        public static Task ChantAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            // Assign default sentence if empty
            if (string.IsNullOrEmpty(argument))
                argument = "Make it happen";

            // Make sure it ends with '!'
            if (!argument.EndsWith('!'))
                argument = $"{argument}!";

            // CONVERT TO UPPERCASE and escape
            argument = ChatHelper.EscapePlaintextAsMarkdownV2(argument.ToUpper());

            // Apply bold format and repeat
            argument = $"*{argument}*{Environment.NewLine}*{argument}*{Environment.NewLine}*{argument}*";

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  argument,
                                                  parseMode: ParseMode.MarkdownV2,
                                                  cancellationToken: cancellationToken);
        }

        public Task DrinkAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} drank {targetMessage.From.FirstName}! 🥤🤤",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} drank {targetName}! 🥤🤤",
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                var beverageIndex = _random.Next(Beverages.Length);
                var beverage = Beverages[beverageIndex];
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      beverage,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task SayThankAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"Thank you so much, {targetMessage.From.FirstName}! 😊",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"Thank you so much, {targetName}! 😊",
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      "You must either reply to a message or specify someone to thank!",
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task SayThanksAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              "You're welcome! 🦾",
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);

        public static Task VaccinateAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              "💉",
                                              replyToMessageId: message.ReplyToMessage?.MessageId ?? 0,
                                              cancellationToken: cancellationToken);
    }
}
