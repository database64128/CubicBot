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

        public QueryStats(Config config)
        {
            Commands.Add(new("get_stats", "📅 View your stats in this chat, or reply to a message to view the sender's stats in this chat.", QueryUserStats, userOrMemberRespondAsync: SendUserStats));

            if (config.Stats.EnableCommandStats)
            {
                if (config.Commands.EnableChinese)
                {
                    Commands.Add(new("leaderboard_interrogations", "🔫 发起喝茶排行榜", SendInterrogationsInitiatedLeaderboardAsync));
                    Commands.Add(new("leaderboard_interrogated", "☕ 被请喝茶排行榜", SendInterrogatedLeaderboardAsync));
                }
            }

            if (config.Stats.EnableGrass)
            {
                Commands.Add(new("leaderboard_grass", "🍀 View grass growth rankings in this chat.", SendGrassGrownLeaderboardAsync));
            }
        }

        public static Task QueryUserStats(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default) => Task.CompletedTask;

        private static Task SendUserStats(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, UserData userData, GroupData? groupData, UserData? replyToUserData, CancellationToken cancellationToken = default)
        {
            var targetUserData = replyToUserData ?? userData;

            var responseSB = new StringBuilder();

            responseSB.AppendLine($"Messages processed: {targetUserData.MessagesProcessed}");
            responseSB.AppendLine($"Commands handled: {targetUserData.CommandsHandled}");

            #region 1. Common
            if (config.Commands.EnableCommon)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("1. Common");
                responseSB.AppendLine($"Apologies sent: {targetUserData.ApologiesSent}");
                responseSB.AppendLine($"Apologies received: {targetUserData.ApologiesReceived}");
                responseSB.AppendLine($"Chants: {targetUserData.ChantsUsed}");
                responseSB.AppendLine($"Drinks taken: {targetUserData.DrinksTaken}");
                responseSB.AppendLine($"Times drank by others: {targetUserData.DrankByOthers}");
                responseSB.AppendLine($"\"/me\" times: {targetUserData.MesUsed}");
                responseSB.AppendLine($"Thank-yous sent: {targetUserData.ThankYousSent}");
                responseSB.AppendLine($"Thank-yous received: {targetUserData.ThankYousReceived}");
                responseSB.AppendLine($"Thanks said: {targetUserData.ThanksSaid}");
                responseSB.AppendLine($"Vaccination shots administered: {targetUserData.VaccinationShotsAdministered}");
                responseSB.AppendLine($"Vaccination shots taken: {targetUserData.VaccinationShotsGot}");
            }
            #endregion

            #region 2. Dice
            if (config.Commands.EnableDice)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("2. Dice");
                responseSB.AppendLine($"Dices thrown: {targetUserData.DicesThrown}");
                responseSB.AppendLine($"Darts thrown: {targetUserData.DartsThrown}");
                responseSB.AppendLine($"Basketballs thrown: {targetUserData.BasketballsThrown}");
                responseSB.AppendLine($"Soccer Goals: {targetUserData.SoccerGoals}");
                responseSB.AppendLine($"Slots rolled: {targetUserData.SlotMachineRolled}");
                responseSB.AppendLine($"Pins knocked: {targetUserData.PinsKnocked}");
            }
            #endregion

            #region 3. Consent Not Needed
            if (config.Commands.EnableConsentNotNeeded)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("3. Consent Not Needed");
                responseSB.AppendLine($"Meals cooked: {targetUserData.MealsCooked}");
                responseSB.AppendLine($"Cooked as meals: {targetUserData.CookedByOthers}");
                responseSB.AppendLine($"Times using force: {targetUserData.ForceUsed}");
                responseSB.AppendLine($"Times being forced: {targetUserData.ForcedByOthers}");
                responseSB.AppendLine($"Times touching someone: {targetUserData.TouchesGiven}");
                responseSB.AppendLine($"Times being touched: {targetUserData.TouchesReceived}");
                responseSB.AppendLine($"Times initiating sex: {targetUserData.SexInitiated}");
                responseSB.AppendLine($"Times accepting sex: {targetUserData.SexReceived}");
            }
            #endregion

            #region 4. Not A Vegan
            if (config.Commands.EnableNonVegan)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("4. Not A Vegan");
                responseSB.AppendLine($"Food eaten: {targetUserData.FoodEaten}");
                responseSB.AppendLine($"Eaten as food: {targetUserData.EatenByOthers}");
            }
            #endregion

            #region 5. Law Enforcement
            if (config.Commands.EnableLawEnforcement)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("5. Law Enforcement");
                responseSB.AppendLine($"Cop calls: {targetUserData.CopCallsMade}");
                responseSB.AppendLine($"Arrests made: {targetUserData.ArrestsMade}");
                responseSB.AppendLine($"Arrests received: {targetUserData.ArrestsReceived}");
                responseSB.AppendLine($"Verdicts given: {targetUserData.VerdictsGiven}");
                responseSB.AppendLine($"Verdicts received: {targetUserData.VerdictsReceived}");
            }
            #endregion

            #region 6. Public Services
            if (config.Commands.EnablePublicServices)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("6. Public Services");
                responseSB.AppendLine($"Ambulances called: {targetUserData.AmbulancesCalled}");
                responseSB.AppendLine($"Fires reported: {targetUserData.FiresReported}");
            }
            #endregion

            #region 7. Chinese
            if (config.Commands.EnableChinese)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("7. 查水表");
                responseSB.AppendLine($"发起喝茶次数: {targetUserData.InterrogationsInitiated}");
                responseSB.AppendLine($"被请喝茶次数: {targetUserData.InterrogatedByOthers}");
            }
            #endregion

            #region 8. Chinese Tasks
            if (config.Commands.EnableChineseTasks)
            {
                responseSB.AppendLine();
                responseSB.AppendLine("8. 任务");
                responseSB.AppendLine($"OKs said: {targetUserData.OkaysSaid}");
                responseSB.AppendLine($"OKs received: {targetUserData.OkaysReceived}");
                responseSB.AppendLine($"Assignments created: {targetUserData.AssignmentsCreated}");
                responseSB.AppendLine($"Assignments received: {targetUserData.AssignmentsReceived}");
                responseSB.AppendLine($"Times unassigning: {targetUserData.UnassignInitiated}");
                responseSB.AppendLine($"Times being unassigned: {targetUserData.UnassignReceived}");
            }
            #endregion

            if (config.Stats.EnableGrass)
            {
                responseSB.AppendLine();
                responseSB.AppendLine($"生草数量: {targetUserData.GrassGrown}");
            }

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  responseSB.ToString(),
                                                  replyToMessageId: message.MessageId,
                                                  cancellationToken: cancellationToken);
        }

        public static Task SendInterrogationsInitiatedLeaderboardAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => SendLeaderboardAsync(botClient,
                                    message,
                                    argument,
                                    data,
                                    "发起喝茶排行榜",
                                    x => x.InterrogationsInitiated,
                                    x => x.InterrogationsInitiated,
                                    cancellationToken);

        public static Task SendInterrogatedLeaderboardAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => SendLeaderboardAsync(botClient,
                                    message,
                                    argument,
                                    data,
                                    "被请喝茶排行榜",
                                    x => x.Members.Select(x => x.Value.InterrogatedByOthers)
                                                  .Aggregate(0Ul, (x, y) => x + y),
                                    x => x.InterrogatedByOthers,
                                    cancellationToken);

        public static Task SendGrassGrownLeaderboardAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => SendLeaderboardAsync(botClient,
                                    message,
                                    argument,
                                    data,
                                    "生草排行榜",
                                    x => x.Members.Select(x => x.Value.GrassGrown)
                                                  .Aggregate(0Ul, (x, y) => x + y),
                                    x => x.GrassGrown,
                                    cancellationToken);

        private static async Task SendLeaderboardAsync(
            ITelegramBotClient botClient,
            Message message,
            string? argument,
            Data data,
            string? title,
            Func<GroupData, ulong>? getTotal,
            Func<UserData, ulong> getStats,
            CancellationToken cancellationToken = default)
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

            var generateLeaderboardTasks = groupData.Members.OrderByDescending(x => getStats(x.Value))
                                                            .Take(10)
                                                            .Select(async x => (await GetChatMemberFirstName(botClient, message.Chat.Id, x.Key, cancellationToken), getStats(x.Value)));
            (string firstName, ulong stats)[]? leaderboard = (await Task.WhenAll(generateLeaderboardTasks)).ToArray();

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

            if (!string.IsNullOrEmpty(title))
            {
                replyBuilder.AppendLine($"*{ChatHelper.EscapeMarkdownV2Plaintext(title)}*");
            }

            if (getTotal is not null)
            {
                replyBuilder.AppendLine($"Total: *{getTotal(groupData)}*");
            }

            var maxNameLength = leaderboard.Max(x => x.firstName.Length);

            replyBuilder.AppendLine("```");

            for (var i = 0; i < leaderboard.Length; i++)
            {
                var barsCount = 10UL * leaderboard[i].stats / leaderboard[0].stats;
                var bars = new string('█', Convert.ToInt32(barsCount));
                replyBuilder.AppendLine($"{i + 1,2}. {ChatHelper.EscapeMarkdownV2CodeBlock(leaderboard[i].firstName).PadRight(maxNameLength)} {leaderboard[i].stats,10} {bars}");
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
