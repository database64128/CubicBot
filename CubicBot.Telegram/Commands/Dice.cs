﻿using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Dice
{
    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("dice", "🎲 Dice it!", SendDiceAsync, statsCollector: CountDices));
        commands.Add(new("dart", "🎯 Oh shoot!", SendDartAsync, statsCollector: CountDarts));
        commands.Add(new("basketball", "🏀 404 Basket Not Found", SendBasketballAsync, statsCollector: CountBasketballsThrown));
        commands.Add(new("soccer", "⚽ It's your goal!", SendSoccerBallAsync, statsCollector: CountSoccerGoals));
        commands.Add(new("roll", "🎰 Feeling unlucky as hell?", SendSlotMachineAsync, statsCollector: CountSlotRolls));
        commands.Add(new("bowl", "🎳 Can you knock them all down?", SendBowlingBallAsync, statsCollector: CountBowlingBalls));
    }

    private static int GetCountFromArgument(string? argument = null)
    {
        if (int.TryParse(argument, out var specifiedCount) && specifiedCount is > 0 and <= 7)
            return specifiedCount;
        else
            return Random.Shared.Next(1, 4);
    }

    private static Task SendAnimatedEmojiAsync(CommandContext commandContext, string emoji, CancellationToken cancellationToken = default)
    {
        int count = GetCountFromArgument(commandContext.Argument);
        Span<Task> tasks = new Task[count];

        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = commandContext.SendDiceWithRetryAsync(emoji, disableNotification: true, cancellationToken: cancellationToken);
        }

        return Task.WhenAll(tasks);
    }

    public static Task SendDiceAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, DiceEmoji.Dice, cancellationToken);

    public static void CountDices(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.DicesThrown++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.DicesThrown++;
        }
    }

    public static Task SendDartAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, DiceEmoji.Darts, cancellationToken);

    public static void CountDarts(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.DartsThrown++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.DartsThrown++;
        }
    }

    public static Task SendBasketballAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, DiceEmoji.Basketball, cancellationToken);

    public static void CountBasketballsThrown(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.BasketballsThrown++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.BasketballsThrown++;
        }
    }

    public static Task SendSoccerBallAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, DiceEmoji.Football, cancellationToken);

    public static void CountSoccerGoals(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.SoccerGoals++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.SoccerGoals++;
        }
    }

    public static Task SendSlotMachineAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, DiceEmoji.SlotMachine, cancellationToken);

    public static void CountSlotRolls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.SlotMachineRolled++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.SlotMachineRolled++;
        }
    }

    public static Task SendBowlingBallAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, DiceEmoji.Bowling, cancellationToken);

    public static void CountBowlingBalls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.PinsKnocked++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.PinsKnocked++;
        }
    }
}
