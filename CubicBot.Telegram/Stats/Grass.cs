using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Stats
{
    public class Grass : IStatsCollector
    {
        public static string[] GrassSeeds => new string[]
        {
            "cao", "艹", "草", "c奥", "c嗷",
        };

        private static bool IsGrowingGrass(string msg) => GrassSeeds.Any(seed => msg.Contains(seed));

        public Task CollectAsync(ITelegramBotClient botClient, Message message, Data data, CancellationToken cancellationToken = default)
            => string.IsNullOrEmpty(message.Text) || !IsGrowingGrass(message.Text)
                ? Task.CompletedTask
                : message.Chat.Type is ChatType.Private
                ? UpdateUserGrassGrownAsync(botClient, message, data.Users, cancellationToken)
                : UpdateGroupGrassGrownAsync(botClient, message, data.Groups, cancellationToken);

        private static Task UpdateGroupGrassGrownAsync(ITelegramBotClient botClient, Message message, Dictionary<long, GroupData> groupDataDict, CancellationToken cancellationToken = default)
        {
            var groupId = message.Chat.Id;
            if (groupDataDict.TryGetValue(groupId, out var groupData))
            {
                return UpdateUserGrassGrownAsync(botClient, message, groupData.Members, cancellationToken);
            }
            else
            {
                var newGroupData = new GroupData();
                groupDataDict.Add(groupId, newGroupData);
                return UpdateUserGrassGrownAsync(botClient, message, newGroupData.Members, cancellationToken);
            }
        }

        private static Task UpdateUserGrassGrownAsync(ITelegramBotClient botClient, Message message, Dictionary<long, UserData> userDataDict, CancellationToken cancellationToken = default)
        {
            var userId = message.From.Id;
            if (userDataDict.TryGetValue(userId, out var userData))
            {
                return UpdateGrassGrownAsync(botClient, message, userData, cancellationToken);
            }
            else
            {
                var newUserData = new UserData();
                userDataDict.Add(userId, newUserData);
                return UpdateGrassGrownAsync(botClient, message, newUserData, cancellationToken);
            }
        }

        private static Task UpdateGrassGrownAsync(ITelegramBotClient botClient, Message message, UserData userData, CancellationToken cancellationToken = default)
        {
            userData.GrassGrown++;

            return userData.GrassGrown switch
            {
                10UL or 20UL or 50UL or 100UL or 200UL or 500UL or 1000UL or 2000UL or 5000UL or 10000UL => NotifyGrassGrowthAchievementAsync(botClient, message, userData.GrassGrown, cancellationToken),
                _ => Task.CompletedTask,
            };
        }

        public static Task NotifyGrassGrowthAchievementAsync(ITelegramBotClient botClient, Message message, ulong count, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id, $"🏆 Achievement Unlocked: {count} Grass Grown", replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
    }
}
