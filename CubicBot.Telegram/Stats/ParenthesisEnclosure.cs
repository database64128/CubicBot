﻿using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats;

public class ParenthesisEnclosure : UserStatsCollector
{
    private readonly List<char> _compensation = new();

    public static readonly Dictionary<char, char> EnclosureDict = new()
    {
        // bi-directional
        ['"'] = '"',

        // left -> right
        ['('] = ')',
        ['['] = ']',
        ['{'] = '}',
        ['<'] = '>',
        ['（'] = '）',
        ['【'] = '】',
        ['「'] = '」',
        ['《'] = '》',
        ['“'] = '”',

        // right -> left
        [')'] = '(',
        [']'] = '[',
        ['}'] = '{',
        ['>'] = '<',
        ['）'] = '（',
        ['】'] = '【',
        ['」'] = '「',
        ['》'] = '《',
        ['”'] = '“',
    };

    public bool AnalyzeMessage(ReadOnlySpan<char> msg)
    {
        _compensation.Clear();

        for (var i = 0; i < msg.Length; i++)
        {
            // Short-circuit 'x'.
            if (msg[i] == '\'' && i + 2 < msg.Length && msg[i + 2] == '\'')
            {
                i += 2;
                continue;
            }

            // Skip enclosed double quotes.
            if (msg[i] == '"' && i + 1 < msg.Length)
            {
                var nextDoubleQuote = msg[(i + 1)..].IndexOf('"');
                if (nextDoubleQuote != -1)
                {
                    i += 1 + nextDoubleQuote;
                    continue;
                }
            }

            // Handle enclosure signs.
            if (EnclosureDict.TryGetValue(msg[i], out var otherHalf))
            {
                var lastThisHalfMatch = _compensation.LastIndexOf(msg[i]);
                if (lastThisHalfMatch != -1)
                {
                    _compensation.RemoveAt(lastThisHalfMatch);
                }
                else
                {
                    _compensation.Add(otherHalf);
                }
            }
        }

        return _compensation.Count > 0;
    }

    public string GetCompensationString() => new(CollectionsMarshal.AsSpan(_compensation));

    public override bool IsCollectable(Message message) => AnalyzeMessage(ChatHelper.GetMessageText(message));

    public override void CollectUser(Message message, UserData userData, GroupData? groupData) =>
        userData.ParenthesesUnenclosed += (ulong)_compensation.Count;

    public override Task RespondAsync(ITelegramBotClient botClient, Message message, UserData userData, GroupData? groupData, CancellationToken cancellationToken)
    {
        var ensureParenthesisEnclosure = groupData switch
        {
            null => userData.EnsureParenthesisEnclosure,
            _ => groupData.EnsureParenthesisEnclosure,
        };

        return ensureParenthesisEnclosure && _compensation.Count > 0
            ? botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                      GetCompensationString(),
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken)
            : Task.CompletedTask;
    }
}
