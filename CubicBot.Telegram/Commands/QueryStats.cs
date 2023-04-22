using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public sealed class QueryStats
{
    private readonly Config _config;

    public ReadOnlyCollection<CubicBotCommand> Commands { get; }

    public QueryStats(Config config)
    {
        _config = config;

        var commands = new List<CubicBotCommand>()
        {
            new("get_stats", "📅 View your stats in this chat, or reply to a message to view the sender's stats in this chat.", QueryUserStats),
        };

        if (config.Stats.EnableCommandStats)
        {
            if (config.Commands.EnableCommon)
            {
                commands.Add(new("leaderboard_apologetic", "🙏 Who's the most apologetic person in this chat?", SendApologeticLeaderboardAsync));
                commands.Add(new("leaderboard_apologies_accepted", "😭 Who got the most apologies in this chat?", SendApologiesAcceptedLeaderboardAsync));
                commands.Add(new("leaderboard_chant", "📢 Who chants the most in this chat?", SendChantsLeaderboardAsync));
                commands.Add(new("leaderboard_drink", "🍺 Who drinks the most in this chat?", SendDrinkLeaderboardAsync));
                commands.Add(new("leaderboard_me", "🤳 Who loves themselves the most in this chat?", SendLoveThemselvesLeaderboardAsync));
                commands.Add(new("leaderboard_thankful", "😊 Who's the most thankful person in this chat?", SendThankfulLeaderboardAsync));
                commands.Add(new("leaderboard_appreciated", "💖 Who's the most appreciated person in this chat?", SendAppreciatedLeaderboardAsync));
            }

            if (config.Commands.EnableDice)
            {
                commands.Add(new("leaderboard_dice", "🎲 View dice rankings in this chat.", SendDicesLeaderboardAsync));
                commands.Add(new("leaderboard_dart", "🎯 View dart rankings in this chat.", SendDartsLeaderboardAsync));
                commands.Add(new("leaderboard_basketball", "🏀 View basketball rankings in this chat.", SendBasketballsLeaderboardAsync));
                commands.Add(new("leaderboard_soccer", "⚽ View soccer rankings in this chat.", SendSoccerGoalsLeaderboardAsync));
                commands.Add(new("leaderboard_roll", "🎰 View slot machine rankings in this chat.", SendSlotsRolledLeaderboardAsync));
                commands.Add(new("leaderboard_bowl", "🎳 View bowling rankings in this chat.", SendPinsKnockedLeaderboardAsync));
            }

            if (config.Commands.EnableConsentNotNeeded)
            {
                commands.Add(new("leaderboard_sexual", "💋 Who has the most sex in this chat?", SendSexualLeaderboardAsync));
            }

            if (config.Commands.EnableLawEnforcement)
            {
                commands.Add(new("leaderboard_criminal", "🦹 View criminal rankings in this chat.", SendCriminalLeaderboardAsync));
            }

            if (config.Commands.EnableChinese)
            {
                commands.Add(new("leaderboard_interrogations", "🔫 发起喝茶排行榜", SendInterrogationsInitiatedLeaderboardAsync));
                commands.Add(new("leaderboard_interrogated", "☕ 被请喝茶排行榜", SendInterrogatedLeaderboardAsync));
            }

            if (config.Commands.EnableSystemd)
            {
                commands.Add(new("leaderboard_systemd", "🐧 Who's the biggest systemd fan in this chat?", SendSystemdFandomLeaderboardAsync));
            }
        }

        if (config.Stats.EnableGrass)
        {
            commands.Add(new("leaderboard_grass", "🍀 View grass growth rankings in this chat.", SendGrassGrownLeaderboardAsync));
        }

        if (config.Stats.EnableCommandStats)
        {
            commands.Add(new("leaderboard_demanding", "👉 Who's the most demanding person in this chat?", SendDemandingLeaderboardAsync));
        }

        if (config.Stats.EnableMessageCounter)
        {
            commands.Add(new("leaderboard_talkative", "🗣️ Who's the most talkative person in this chat?", SendTalkativeLeaderboardAsync));
        }

        if (config.Stats.EnableParenthesisEnclosure)
        {
            commands.Add(new("leaderboard_half_parentheses", "🌓 括号发一半排行榜", SendParenthesesUnenclosedLeaderboardAsync));
        }

        Commands = commands.AsReadOnly();
    }

    public Task QueryUserStats(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var targetUserData = commandContext.ReplyToMessageContext?.MemberOrUserData ?? commandContext.MemberOrUserData;

        var responseSB = new StringBuilder();

        if (_config.Stats.EnableMessageCounter)
        {
            responseSB.AppendLine($"Messages processed: {targetUserData.MessagesProcessed}");
        }

        if (_config.Stats.EnableCommandStats)
        {
            responseSB.AppendLine($"Commands handled: {targetUserData.CommandsHandled}");
        }

        if (_config.Stats.EnableGrass)
        {
            responseSB.AppendLine($"生草数量: {targetUserData.GrassGrown}");
        }

        if (_config.Stats.EnableParenthesisEnclosure)
        {
            responseSB.AppendLine($"括号发一半数量: {targetUserData.ParenthesesUnenclosed}");
        }

        #region 1. Common
        if (_config.Commands.EnableCommon)
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
            responseSB.AppendLine($"Vaccination shots received: {targetUserData.VaccinationShotsReceived}");
        }
        #endregion

        #region 2. Dice
        if (_config.Commands.EnableDice)
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
        if (_config.Commands.EnableConsentNotNeeded)
        {
            responseSB.AppendLine();
            responseSB.AppendLine("3. Consent Not Needed");
            responseSB.AppendLine($"Meals cooked: {targetUserData.MealsCooked}");
            responseSB.AppendLine($"Cooked as meals: {targetUserData.CookedByOthers}");
            responseSB.AppendLine($"Times throwing people: {targetUserData.PersonsThrown}");
            responseSB.AppendLine($"Times being thrown: {targetUserData.ThrownByOthers}");
            responseSB.AppendLine($"Times catching people: {targetUserData.PersonsCaught}");
            responseSB.AppendLine($"Times being caught: {targetUserData.CaughtByOthers}");
            responseSB.AppendLine($"Times using force: {targetUserData.ForceUsed}");
            responseSB.AppendLine($"Times being forced: {targetUserData.ForcedByOthers}");
            responseSB.AppendLine($"Times touching someone: {targetUserData.TouchesGiven}");
            responseSB.AppendLine($"Times being touched: {targetUserData.TouchesReceived}");
            responseSB.AppendLine($"Times initiating sex: {targetUserData.SexInitiated}");
            responseSB.AppendLine($"Times accepting sex: {targetUserData.SexReceived}");
        }
        #endregion

        #region 4. Not A Vegan
        if (_config.Commands.EnableNonVegan)
        {
            responseSB.AppendLine();
            responseSB.AppendLine("4. Not A Vegan");
            responseSB.AppendLine($"Food eaten: {targetUserData.FoodEaten}");
            responseSB.AppendLine($"Eaten as food: {targetUserData.EatenByOthers}");
        }
        #endregion

        #region 5. Law Enforcement
        if (_config.Commands.EnableLawEnforcement)
        {
            responseSB.AppendLine();
            responseSB.AppendLine("5. Law Enforcement");
            responseSB.AppendLine($"Cop calls: {targetUserData.CopCallsMade}");
            responseSB.AppendLine($"Arrests made: {targetUserData.ArrestsMade}");
            responseSB.AppendLine($"Arrests received: {targetUserData.ArrestsReceived}");
            responseSB.AppendLine($"Verdicts given: {targetUserData.VerdictsGiven}");
            responseSB.AppendLine($"Verdicts received: {targetUserData.VerdictsReceived}");
            responseSB.AppendLine($"Overthrow attempts: {targetUserData.OverthrowAttempts}");
            responseSB.AppendLine($"Overthrow attempts received: {targetUserData.OverthrowAttemptsReceived}");
        }
        #endregion

        #region 6. Public Services
        if (_config.Commands.EnablePublicServices)
        {
            responseSB.AppendLine();
            responseSB.AppendLine("6. Public Services");
            responseSB.AppendLine($"Ambulances called: {targetUserData.AmbulancesCalled}");
            responseSB.AppendLine($"Fires reported: {targetUserData.FiresReported}");
        }
        #endregion

        #region 7. Chinese
        if (_config.Commands.EnableChinese)
        {
            responseSB.AppendLine();
            responseSB.AppendLine("7. 查水表");
            responseSB.AppendLine($"发起喝茶次数: {targetUserData.InterrogationsInitiated}");
            responseSB.AppendLine($"被请喝茶次数: {targetUserData.InterrogatedByOthers}");
        }
        #endregion

        #region 8. Chinese Tasks
        if (_config.Commands.EnableChineseTasks)
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

        #region 9. Systemd
        if (_config.Commands.EnableSystemd)
        {
            responseSB.AppendLine();
            responseSB.AppendLine("9. Systemd");
            responseSB.AppendLine($"Systemctl commands used: {targetUserData.SystemctlCommandsUsed}");
        }
        #endregion

        if (responseSB.Length == 0)
        {
            responseSB.Append("No stats.");
        }

        return commandContext.ReplyWithTextMessageAndRetryAsync(responseSB.ToString(), cancellationToken: cancellationToken);
    }

    #region 1. Common
    public static Task SendApologeticLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🙏 Who's the most apologetic person in this chat?",
                                null,
                                x => x.ApologiesSent,
                                cancellationToken);

    public static Task SendApologiesAcceptedLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "😭 Apologies Accepted",
                                null,
                                x => x.ApologiesReceived,
                                cancellationToken);

    public static Task SendChantsLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "📢 Who chants the most in this chat?",
                                null,
                                x => x.ChantsUsed,
                                cancellationToken);

    public static Task SendDrinkLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🍺 Who drinks the most in this chat?",
                                null,
                                x => x.DrinksTaken,
                                cancellationToken);

    public static Task SendLoveThemselvesLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🤳 Who loves themselves the most in this chat?",
                                null,
                                x => x.MesUsed,
                                cancellationToken);

    public static Task SendThankfulLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "😊 Who's the most thankful person in this chat?",
                                null,
                                x => x.ThankYousSent,
                                cancellationToken);

    public static Task SendAppreciatedLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "💖 Who's the most appreciated person in this chat?",
                                null,
                                x => x.ThankYousReceived,
                                cancellationToken);
    #endregion

    #region 2. Dice
    public static Task SendDicesLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🎲 Dices Thrown",
                                x => x.DicesThrown,
                                x => x.DicesThrown,
                                cancellationToken);

    public static Task SendDartsLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🎯 Darts Thrown",
                                x => x.DartsThrown,
                                x => x.DartsThrown,
                                cancellationToken);

    public static Task SendBasketballsLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🏀 Basketballs Thrown",
                                x => x.BasketballsThrown,
                                x => x.BasketballsThrown,
                                cancellationToken);

    public static Task SendSoccerGoalsLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "⚽ Soccer Goals",
                                x => x.SoccerGoals,
                                x => x.SoccerGoals,
                                cancellationToken);

    public static Task SendSlotsRolledLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🎰 Slots Rolled",
                                x => x.SlotMachineRolled,
                                x => x.SlotMachineRolled,
                                cancellationToken);

    public static Task SendPinsKnockedLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🎳 Pins Knocked",
                                x => x.PinsKnocked,
                                x => x.PinsKnocked,
                                cancellationToken);
    #endregion

    #region 3. Consent Not Needed
    public static Task SendSexualLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "💋 Who's the most sexual person in this chat?",
                                x => x.SexInitiated,
                                x => x.SexInitiated,
                                cancellationToken);
    #endregion

    #region 5. Law Enforcement
    public static Task SendCriminalLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🦹 Criminals",
                                x => x.ArrestsMade,
                                x => x.ArrestsReceived,
                                cancellationToken);
    #endregion

    #region 7. Chinese
    public static Task SendInterrogationsInitiatedLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🔫 发起喝茶排行榜",
                                x => x.InterrogationsInitiated,
                                x => x.InterrogationsInitiated,
                                cancellationToken);

    public static Task SendInterrogatedLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "☕ 被请喝茶排行榜",
                                x => x.Members.Select(x => x.Value.InterrogatedByOthers)
                                              .Aggregate(0Ul, (x, y) => x + y),
                                x => x.InterrogatedByOthers,
                                cancellationToken);
    #endregion

    #region 9. Systemd
    public static Task SendSystemdFandomLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🐧 Who's the biggest systemd fan in this chat?",
                                null,
                                x => x.SystemctlCommandsUsed,
                                cancellationToken);
    #endregion

    public static Task SendGrassGrownLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🌿 生草排行榜",
                                x => x.Members.Select(x => x.Value.GrassGrown)
                                              .Aggregate(0Ul, (x, y) => x + y),
                                x => x.GrassGrown,
                                cancellationToken);

    public static Task SendDemandingLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "👉 Who's the most demanding person in this chat?",
                                x => x.CommandsHandled,
                                x => x.CommandsHandled,
                                cancellationToken);

    public static Task SendTalkativeLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🗣️ Who's the most talkative person in this chat?",
                                x => x.MessagesProcessed,
                                x => x.MessagesProcessed,
                                cancellationToken);

    public static Task SendParenthesesUnenclosedLeaderboardAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendLeaderboardAsync(commandContext,
                                "🌓 括号发一半排行榜",
                                x => x.Members.Select(x => x.Value.ParenthesesUnenclosed)
                                              .Aggregate(0Ul, (x, y) => x + y),
                                x => x.ParenthesesUnenclosed,
                                cancellationToken);

    private static async Task SendLeaderboardAsync(
        CommandContext commandContext,
        string? title,
        Func<GroupData, ulong>? getTotal,
        Func<UserData, ulong> getStats,
        CancellationToken cancellationToken = default)
    {
        var groupData = commandContext.GroupData;
        if (groupData is null)
        {
            await commandContext.ReplyWithTextMessageAndRetryAsync("This command can only be used in group chats.", cancellationToken: cancellationToken);
            return;
        }

        var sendDummyReplyTask = commandContext.ReplyWithTextMessageAndRetryAsync("Querying user information...", cancellationToken: cancellationToken);

        var generateLeaderboardTasks = groupData.Members.OrderByDescending(x => getStats(x.Value))
                                                        .Take(10)
                                                        .Select(async x => (await GetChatMemberFirstName(commandContext, x.Key, cancellationToken), getStats(x.Value)));
        (string firstName, ulong stats)[] leaderboard = (await Task.WhenAll(generateLeaderboardTasks)).ToArray();

        var dummyReply = await sendDummyReplyTask;

        if (leaderboard.Length == 0 || leaderboard[0].stats == 0UL)
        {
            await commandContext.EditMessageTextWithRetryAsync(
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

        await commandContext.EditMessageTextWithRetryAsync(
            dummyReply.MessageId,
            replyBuilder.ToString(),
            ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private static async Task<string> GetChatMemberFirstName(CommandContext commandContext, long userId, CancellationToken cancellationToken = default)
    {
        string firstName;

        try
        {
            var chatMember = await commandContext.BotClient.GetChatMemberAsync(commandContext.Message.Chat.Id, userId, cancellationToken);
            firstName = chatMember.User.FirstName;
        }
        catch
        {
            firstName = "";
        }

        return firstName;
    }
}
