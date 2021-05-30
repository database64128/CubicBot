using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class Chinese
    {
        public static string[] Questions => new string[]
        {
            "你发这些什么目的？",
            "谁指使你的？",
            "你的动机是什么？",
            "你取得有关部门许可了吗？",
            "法律法规容许你发了吗？",
            "你背后是谁？",
            "发这些想干什么？",
            "你想颠覆什么？",
            "你要破坏什么？",
        };

        public CubicBotCommand[] Commands => new CubicBotCommand[]
        {
            new("interrogate", "🔫 开门，查水表！", InterrogateAsync),
        };

        private readonly Random _random;

        public Chinese(Random random) => _random = random;

        public Task InterrogateAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var randomIndex = _random.Next(Questions.Length);
            var randomQuestion = Questions[randomIndex];

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  randomQuestion,
                                                  replyToMessageId: message.ReplyToMessage?.MessageId ?? 0,
                                                  cancellationToken: cancellationToken);
        }
    }
}
