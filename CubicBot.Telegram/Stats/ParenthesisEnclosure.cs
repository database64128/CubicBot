using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed class ParenthesisEnclosure : IStatsCollector
{
    private readonly List<char> _compensation = new();

    private static readonly Dictionary<char, char> s_enclosureDict = new()
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
            if (s_enclosureDict.TryGetValue(msg[i], out var otherHalf))
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

    public string GetCompensationString()
    {
        var compensation = new string(CollectionsMarshal.AsSpan(_compensation));
        return compensation;
    }

    public Task CollectAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var task = Task.CompletedTask;

        if (AnalyzeMessage(messageContext.Text))
        {
            messageContext.MemberOrUserData.ParenthesesUnenclosed += (ulong)_compensation.Count;

            var ensureParenthesisEnclosure = messageContext.GroupData switch
            {
                null => messageContext.UserData.EnsureParenthesisEnclosure,
                _ => messageContext.GroupData.EnsureParenthesisEnclosure,
            };

            if (ensureParenthesisEnclosure)
            {
                task = messageContext.ReplyWithTextMessageAndRetryAsync(GetCompensationString(), cancellationToken: cancellationToken);
            }

            _compensation.Clear();
        }

        return task;
    }
}
