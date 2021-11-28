using CubicBot.Telegram.Stats;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Dice
{
    public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
    {
            new("dice", "🎲 Dice it!", SendDiceAsync, userOrMemberStatsCollector: CountDices),
            new("dart", "🎯 Oh shoot!", SendDartAsync, userOrMemberStatsCollector: CountDarts),
            new("basketball", "🏀 404 Basket Not Found", SendBasketballAsync, userOrMemberStatsCollector: CountBasketballsThrown),
            new("soccer", "⚽ It's your goal!", SendSoccerBallAsync, userOrMemberStatsCollector: CountSoccerGoals),
            new("roll", "🎰 Feeling unlucky as hell?", SendSlotMachineAsync, userOrMemberStatsCollector: CountSlotRolls),
            new("bowl", "🎳 Can you knock them all down?", SendBowlingBallAsync, userOrMemberStatsCollector: CountBowlingBalls),
    };

    private static int GetCountFromArgument(string? argument = null)
    {
        if (int.TryParse(argument, out var specifiedCount) && specifiedCount is > 0 and <= 7)
            return specifiedCount;
        else
            return Random.Shared.Next(1, 4);
    }

    private static Task SendAnimatedEmojiAsync(ITelegramBotClient botClient, Message message, string? argument, Emoji emoji, CancellationToken cancellationToken = default)
    {
        var count = GetCountFromArgument(argument);
        var tasks = new List<Task>();

        for (var i = 0; i < count; i++)
            tasks.Add(botClient.SendDiceAsync(message.Chat.Id, emoji, disableNotification: true, cancellationToken: cancellationToken));

        return Task.WhenAll(tasks);
    }

    public static Task SendDiceAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(botClient, message, argument, Emoji.Dice, cancellationToken);

    public static void CountDices(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.DicesThrown++;
        if (groupData is not null)
        {
            groupData.DicesThrown++;
        }
    }

    public static Task SendDartAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(botClient, message, argument, Emoji.Darts, cancellationToken);

    public static void CountDarts(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.DartsThrown++;
        if (groupData is not null)
        {
            groupData.DartsThrown++;
        }
    }

    public static Task SendBasketballAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(botClient, message, argument, Emoji.Basketball, cancellationToken);

    public static void CountBasketballsThrown(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.BasketballsThrown++;
        if (groupData is not null)
        {
            groupData.BasketballsThrown++;
        }
    }

    public static Task SendSoccerBallAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(botClient, message, argument, Emoji.Football, cancellationToken);

    public static void CountSoccerGoals(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.SoccerGoals++;
        if (groupData is not null)
        {
            groupData.SoccerGoals++;
        }
    }

    public static Task SendSlotMachineAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(botClient, message, argument, Emoji.SlotMachine, cancellationToken);

    public static void CountSlotRolls(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.SlotMachineRolled++;
        if (groupData is not null)
        {
            groupData.SlotMachineRolled++;
        }
    }

    public static Task SendBowlingBallAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(botClient, message, argument, Emoji.Bowling, cancellationToken);

    public static void CountBowlingBalls(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.PinsKnocked++;
        if (groupData is not null)
        {
            groupData.PinsKnocked++;
        }
    }
}
