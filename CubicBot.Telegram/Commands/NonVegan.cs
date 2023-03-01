﻿using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public static class NonVegan
{
    private static readonly string[] s_food =
    {
        "🍏", "🍎", "🍐", "🍊", "🍋", "🍌", "🍉", "🍇", "🍓", "🫐",
        "🍈", "🍒", "🍑", "🥭", "🍍", "🥥", "🥝", "🍅", "🍆", "🥑",
        "🥦", "🥬", "🥒", "🌶", "🫑", "🌽", "🥕", "🫒", "🧄", "🧅",
        "🥔", "🍠", "🥐", "🥯", "🍞", "🥖", "🥨", "🧀", "🥚", "🍳",
        "🧈", "🥞", "🧇", "🥓", "🥩", "🍗", "🍖", "🦴", "🌭", "🍔",
        "🍟", "🍕", "🫓", "🥪", "🥙", "🧆", "🌮", "🌯", "🫔", "🥗",
        "🥘", "🫕", "🥫", "🍝", "🍜", "🍲", "🍛", "🍣", "🍱", "🥟",
        "🦪", "🍤", "🍙", "🍚", "🍘", "🍥", "🥠", "🥮", "🍢", "🍡",
        "🍧", "🍨", "🍦", "🥧", "🧁", "🍰", "🎂", "🍮", "🍭", "🍬",
        "🍫", "🍿", "🍩", "🍪", "🌰", "🥜", "🍯",
    };

    public static readonly ReadOnlyCollection<CubicBotCommand> Commands = new(new CubicBotCommand[]
    {
        new("eat", "☃️ Do you want to eat a snowman?", EatAsync, userOrMemberStatsCollector: CountEats),
    });

    public static Task EatAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        if (message.ReplyToMessage is Message targetMessage)
        {
            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                           $"{message.From?.FirstName} ate {targetMessage.From?.FirstName}! 🍴😋",
                                                           replyToMessageId: targetMessage.MessageId,
                                                           cancellationToken: cancellationToken);
        }
        else if (argument is string targetName)
        {
            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                           $"{message.From?.FirstName} ate {targetName}! 🍴😋",
                                                           cancellationToken: cancellationToken);
        }
        else
        {
            var foodIndex = Random.Shared.Next(s_food.Length);
            var food = s_food[foodIndex];
            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                           food,
                                                           replyToMessageId: message.MessageId,
                                                           cancellationToken: cancellationToken);
        }
    }

    public static void CountEats(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.FoodEaten++;
        if (replyToUserData is not null)
        {
            replyToUserData.EatenByOthers++;
        }
    }
}
