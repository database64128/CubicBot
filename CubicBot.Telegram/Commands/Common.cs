using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public static class Common
    {
        public static readonly string[] Beverages = new string[]
        {
            "🍼", "🥛", "☕️", "🫖", "🍵", "🍶", "🍾", "🍷", "🍸", "🍹",
            "🍺", "🍻", "🥂", "🧉", "🏺", "🚰", "🧋", "🧃",
        };

        public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
        {
            new("apologize", "🥺 Sorry about last night.", ApologizeAsync),
            new("chant", "🗣 Say it out loud!", ChantAsync),
            new("drink", "🥤 I'm thirsty!", DrinkAsync),
            new("me", "🤳 What the hell am I doing?", MeAsync),
            new("thank", "🦃 Reply to or mention the name of the person you would like to thank.", SayThankAsync),
            new("thanks", "😊 Say thanks to me!", SayThanksAsync),
            new("vax", "💉 Gen Z also got the vax!", VaccinateAsync),
        };

        public static Task ApologizeAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var apologyStart = Random.Shared.Next(5) switch
            {
                0 => "Sorry",
                1 => "I'm sorry",
                2 => "I'm so sorry",
                3 => "I apologize",
                _ => "I want to apologize",
            };

            if (message.ReplyToMessage is Message targetMessage)
            {
                if (!string.IsNullOrEmpty(argument))
                    argument = $" for {argument}";

                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From?.FirstName}: {apologyStart}{argument}, {targetMessage.From?.FirstName}. 🥺",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From?.FirstName}: {apologyStart}, {targetName}. 🥺",
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{apologyStart}, {message.From?.FirstName}. 🥺",
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task ChantAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            // Assign default sentence if empty
            if (string.IsNullOrEmpty(argument))
            {
                argument = Random.Shared.Next(9) switch
                {
                    0 => "Make it happen",
                    1 => "Do it now",
                    2 => "Love wins",
                    3 => "My body, my choice!",
                    4 => "No justice, no peace!",
                    5 => "No Hate! No Fear! Immigrants are welcome here!",
                    6 => "Climate Change is not a lie, do not let our planet die!",
                    7 => "Waters rise, hear our cries, no more lies for business ties!",
                    _ => "No more secrets, no more lies! No more silence that money buys!",
                };
            }

            // Make sure it ends with '!'
            if (!argument.EndsWith('!'))
                argument = $"{argument}!";

            // CONVERT TO UPPERCASE and escape
            argument = ChatHelper.EscapeMarkdownV2Plaintext(argument.ToUpper());

            // Apply bold format and repeat
            argument = $"*{argument}*{Environment.NewLine}*{argument}*{Environment.NewLine}*{argument}*";

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  argument,
                                                  parseMode: ParseMode.MarkdownV2,
                                                  cancellationToken: cancellationToken);
        }

        public static Task DrinkAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From?.FirstName} drank {targetMessage.From?.FirstName}! 🥤🤤",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From?.FirstName} drank {targetName}! 🥤🤤",
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                var beverageIndex = Random.Shared.Next(Beverages.Length);
                var beverage = Beverages[beverageIndex];
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      beverage,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task MeAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            argument ??= Random.Shared.Next(4) switch
            {
                0 => "did nothing and fell asleep. 😴",
                1 => $"is showing off this new command {data.GetPronounSubject(message.From?.Id ?? 777000L, message.Chat.Type is ChatType.Private ? 0L : message.Chat.Id)} just learned. 😎",
                2 => "got coffee for everyone in this chat. ☕",
                _ => "invoked this command by mistake. 🤪",
            };

            var entities = new MessageEntity[]
            {
                new()
                {
                    Type = MessageEntityType.TextMention,
                    Offset = 2,
                    Length = message.From?.FirstName?.Length ?? 0,
                    User = message.From,
                },
            };

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  $"* {message.From?.FirstName} {argument}",
                                                  entities: entities,
                                                  cancellationToken: cancellationToken);
        }

        public static Task SayThankAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"Thank you so much, {targetMessage.From?.FirstName}! 😊",
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

        public static Task SayThanksAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              "You're welcome! 🦾",
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);

        public static Task VaccinateAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              "💉",
                                              replyToMessageId: message.ReplyToMessage?.MessageId ?? 0,
                                              cancellationToken: cancellationToken);
    }
}
