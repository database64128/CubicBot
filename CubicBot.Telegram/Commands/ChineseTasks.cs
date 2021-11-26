using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class ChineseTasks
    {
        public static string[] OKAnswers => new string[]
        {
            "ok",
            "okay",
            "nice",
            "good",
            "great",
            "thanks",
            "got it",
            "all right",
            "好",
            "好的",
            "好吧",
            "吼",
            "吼的",
            "吼吧",
            "非常好",
            "非常吼",
            "行",
            "可",
            "可以",
            "没问题",
            "完全同意",
            "我觉得好",
            "我  好  了",
            "嗯",
            "嗯！",
            "嗯嗯",
            "嗯嗯！",
            "🉑",
            "👌",
            "🆗",
        };

        public CubicBotCommand[] Commands => new CubicBotCommand[]
        {
            new("ok", "👌 好的，没问题！", OKAsync),
            new("assign", "📛 交给你了！", AssignAsync),
            new("unassign", "💢 不干了！", UnassignAsync),
        };

        private readonly Random _random;

        public ChineseTasks(Random random) => _random = random;

        public Task OKAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var randomIndex = _random.Next(OKAnswers.Length);
            var randomOKAnswer = OKAnswers[randomIndex];

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  randomOKAnswer,
                                                  replyToMessageId: message.ReplyToMessage?.MessageId ?? 0,
                                                  cancellationToken: cancellationToken);
        }

        public async Task AssignAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is null) // self assign
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                                                     $"{message.From?.FirstName}: 交  给  我  了",
                                                     cancellationToken: cancellationToken);
            }
            else // assign to someone else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                                                     "交  给  你  了",
                                                     replyToMessageId: message.ReplyToMessage.MessageId,
                                                     cancellationToken: cancellationToken);

                var randomIndex = _random.Next(OKAnswers.Length);
                var randomOKAnswer = OKAnswers[randomIndex];

                await botClient.SendTextMessageAsync(message.Chat.Id,
                                                     $"{message.ReplyToMessage.From?.FirstName}: {randomOKAnswer}",
                                                     replyToMessageId: message.MessageId,
                                                     cancellationToken: cancellationToken);
            }
        }

        public static Task UnassignAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var targetName = message.ReplyToMessage?.From?.FirstName ?? message.From?.FirstName;
            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  $"{targetName}: 不  干  了",
                                                  replyToMessageId: message.MessageId,
                                                  cancellationToken: cancellationToken);
        }
    }
}
