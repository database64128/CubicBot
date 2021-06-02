using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public class QueryStats
    {
        public List<CubicBotCommand> Commands { get; } = new();

        public QueryStats(StatsConfig statsConfig)
        {
            Commands.Add(new("my_stats", "📅 View your stats in this chat.", QueryUserStats));

            if (statsConfig.EnableCommandStats)
                Commands.Add(new("leaderboard_command", "⌨️ View command usage rankings in this chat.", QueryCommandStats));

            if (statsConfig.EnableGrass)
                Commands.Add(new("leaderboard_grass", "🍀 View grass growth rankings in this chat.", QueryGrassStatsAsync));
        }

        public static Task QueryUserStats(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => message.Chat.Type is ChatType.Private
                ? QueryUserStatsInUserDataDict(botClient, message, message.From.Id, data.Users, cancellationToken)
                : QueryUserStatsInGroupDataDict(botClient, message, message.Chat.Id, message.From.Id, data.Groups, cancellationToken);

        private static Task QueryUserStatsInGroupDataDict(ITelegramBotClient botClient, Message message, long chatId, long userId, Dictionary<long, GroupData> groupDataDict, CancellationToken cancellationToken = default)
            => groupDataDict.TryGetValue(chatId, out var groupData)
                ? QueryUserStatsInUserDataDict(botClient, message, userId, groupData.Members, cancellationToken)
                : botClient.SendTextMessageAsync(message.Chat.Id,
                                                 "No stats.",
                                                 replyToMessageId: message.MessageId,
                                                 cancellationToken: cancellationToken);

        private static Task QueryUserStatsInUserDataDict(ITelegramBotClient botClient, Message message, long userId, Dictionary<long, UserData> userDataDict, CancellationToken cancellationToken = default)
            => userDataDict.TryGetValue(userId, out var userData)
                ? SendUserStats(botClient, message, userData, cancellationToken)
                : botClient.SendTextMessageAsync(message.Chat.Id,
                                                 "No stats.",
                                                 replyToMessageId: message.MessageId,
                                                 cancellationToken: cancellationToken);

        private static Task SendUserStats(ITelegramBotClient botClient, Message message, UserData userData, CancellationToken cancellationToken = default)
        {
            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  $"Grass Grown: {userData.GrassGrown}",
                                                  replyToMessageId: message.MessageId,
                                                  cancellationToken: cancellationToken);
        }

        public static Task QueryCommandStats(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  "Work in progress.",
                                                  replyToMessageId: message.MessageId,
                                                  cancellationToken: cancellationToken);
        }

        public static async Task QueryGrassStatsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            if (message.Chat.Type is ChatType.Private)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                                                      "This command can only be used in group chats.",
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);

                return;
            }

            if (!data.Groups.TryGetValue(message.Chat.Id, out var groupData) || groupData.Members.Count == 0)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                                                      "No stats.",
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);

                return;
            }

            var sendDummyReplyTask = botClient.SendTextMessageAsync(message.Chat.Id,
                                                                    "Querying user information...",
                                                                    replyToMessageId: message.MessageId,
                                                                    cancellationToken: cancellationToken);

            var generateLeaderboardTasks = groupData.Members.OrderByDescending(x => x.Value.GrassGrown)
                                                            .Take(10)
                                                            .Select(async x => (await GetChatMemberFirstName(botClient, message.Chat.Id, x.Key, cancellationToken), x.Value.GrassGrown));
            (string firstName, ulong grassGrown)[]? leaderboard = (await Task.WhenAll(generateLeaderboardTasks)).ToArray();
            var dummyReply = await sendDummyReplyTask;

            if (leaderboard.Length == 0)
            {
                await botClient.EditMessageTextAsync(message.Chat.Id,
                                                     dummyReply.MessageId,
                                                     "No stats.",
                                                     cancellationToken: cancellationToken);

                return;
            }

            var replyBuilder = new StringBuilder();
            var maxNameLength = leaderboard.Max(x => x.firstName.Length);

            replyBuilder.AppendLine("```");

            for (var i = 0; i < leaderboard.Length; i++)
            {
                var barsCount = 10UL * leaderboard[i].grassGrown / leaderboard[0].grassGrown;
                var bars = new string('█', Convert.ToInt32(barsCount));
                replyBuilder.AppendLine($"{i + 1,2}. {ChatHelper.EscapeMarkdownV2CodeBlock(leaderboard[i].firstName).PadRight(maxNameLength)} {leaderboard[i].grassGrown,10} {bars}");
            }

            replyBuilder.AppendLine("```");

            await botClient.EditMessageTextAsync(message.Chat.Id,
                                                 dummyReply.MessageId,
                                                 replyBuilder.ToString(),
                                                 ParseMode.MarkdownV2,
                                                 cancellationToken: cancellationToken);
        }

        private static async Task<string> GetChatMemberFirstName(ITelegramBotClient botClient, ChatId chatId, long userId, CancellationToken cancellationToken = default)
        {
            string firstName;

            try
            {
                var chatMember = await botClient.GetChatMemberAsync(chatId, userId, cancellationToken);
                firstName = chatMember.User.FirstName;
            }
            catch
            {
                firstName = "";
            }

            return firstName;
        }
    }
}
