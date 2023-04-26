using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    private static Task SendAnimatedEmojiAsync(CommandContext commandContext, Emoji emoji, CancellationToken cancellationToken = default)
    {
        var count = GetCountFromArgument(commandContext.Argument);
        var tasks = new Task[count];

        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = (commandContext.SendDiceWithRetryAsync(emoji, disableNotification: true, cancellationToken: cancellationToken));
        }

        return Task.WhenAll(tasks);
    }

    public static Task SendDiceAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, Emoji.Dice, cancellationToken);

    public static void CountDices(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.DicesThrown++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.DicesThrown++;
        }
    }

    public static Task SendDartAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, Emoji.Darts, cancellationToken);

    public static void CountDarts(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.DartsThrown++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.DartsThrown++;
        }
    }

    public static Task SendBasketballAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, Emoji.Basketball, cancellationToken);

    public static void CountBasketballsThrown(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.BasketballsThrown++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.BasketballsThrown++;
        }
    }

    public static Task SendSoccerBallAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, Emoji.Football, cancellationToken);

    public static void CountSoccerGoals(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.SoccerGoals++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.SoccerGoals++;
        }
    }

    public static Task SendSlotMachineAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, Emoji.SlotMachine, cancellationToken);

    public static void CountSlotRolls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.SlotMachineRolled++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.SlotMachineRolled++;
        }
    }

    public static Task SendBowlingBallAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => SendAnimatedEmojiAsync(commandContext, Emoji.Bowling, cancellationToken);

    public static void CountBowlingBalls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.PinsKnocked++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.PinsKnocked++;
        }
    }
}
