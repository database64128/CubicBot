using CubicBot.Telegram.Utils;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats
{
    public class Grass : UserStatsCollector
    {
        public static readonly string[] GrassSeeds = new string[]
        {
            "cao", "艹", "草", "c奥", "c嗷",
        };

        private static bool IsGrowingGrass(string msg) => GrassSeeds.Any(seed => msg.Contains(seed));

        private static Task NotifyGrassGrowthAchievementAsync(ITelegramBotClient botClient, Message message, ulong count, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                       $"🏆 Achievement Unlocked: {count} Grass Grown",
                                                       replyToMessageId: message.MessageId,
                                                       cancellationToken: cancellationToken);

        public override bool IsCollectable(Message message) => !string.IsNullOrEmpty(message.Text) && IsGrowingGrass(message.Text);

        public override void CollectUser(Message message, UserData userData, GroupData? groupData) => userData.GrassGrown++;

        public override Task Respond(ITelegramBotClient botClient, Message message, UserData userData, GroupData? groupData, CancellationToken cancellationToken)
        {
            return userData.GrassGrown switch
            {
                10UL or 20UL or 50UL or 100UL or 200UL or 500UL or 1000UL or 2000UL or 5000UL or 10000UL => NotifyGrassGrowthAchievementAsync(botClient, message, userData.GrassGrown, cancellationToken),
                _ => Task.CompletedTask,
            };
        }
    }
}
