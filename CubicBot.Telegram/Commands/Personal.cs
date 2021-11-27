using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Personal
{
    public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
    {
        new("add_pronouns", "➕ Add pronouns.", AddPronounsAsync),
        new("remove_pronouns", "➖ Remove pronouns.", RemovePronounsAsync),
        new("get_pronouns", "❤️ Get someone's pronouns by replying to someone's message, or display your own pronoun settings.", GetPronounsAsync),
        new("set_default_pronouns", "📛 Set or unset default pronouns for all chats.", SetDefaultPronounsAsync),
        new("set_preferred_pronouns", "🕶️ Set or unset preferred pronouns for this chat.", SetPreferredPronounsAsync),
    };

    public static Task AddPronounsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        const string helpMarkdownV2 = @"This command accepts *subject* _\(they\)_ form, *subject/object* _\(they/them\)_ form, or *subject/object/possessive_pronoun* _\(they/them/theirs\)_ form for common known pronouns\. For uncommon pronouns, you must use the full *subject/object/possessive_determiner/possessive_pronoun/reflexive* _\(they/them/theirs\)_ form\.";
        var userId = message.From?.Id ?? 777000L;

        if (!string.IsNullOrEmpty(argument))
        {
            if (Pronouns.TryParse(argument, out var pronouns))
            {
                var pronounsList = data.GetOrCreateUserData(userId).PronounList;
                if (!pronounsList.Contains(pronouns))
                {
                    pronounsList.Add(pronouns);
                    responseMarkdownV2 = $@"🥳 Successfully added *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* to your list of pronouns\!";
                }
                else
                {
                    responseMarkdownV2 = $@"You already have *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* in your list of pronouns\!";
                }
            }
            else
            {
                responseMarkdownV2 = $@"Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. {helpMarkdownV2}";
            }
        }
        else
        {
            responseMarkdownV2 = helpMarkdownV2;
        }

        return botClient.SendTextMessageAsync(message.Chat.Id,
                                              responseMarkdownV2,
                                              ParseMode.MarkdownV2,
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);
    }

    public static Task RemovePronounsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        var userId = message.From?.Id ?? 777000L;
        var pronounsList = data.GetOrCreateUserData(userId).PronounList;

        if (string.IsNullOrEmpty(argument))
        {
            if (pronounsList.Count == 1)
            {
                var pronouns = pronounsList[0];
                pronounsList.Clear();
                responseMarkdownV2 = $@"🚮 Successfully removed *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* from your list of pronouns\!";
            }
            else
            {
                responseMarkdownV2 = @"You have more than one set of pronouns in your list\. Specify one to remove from the list\.";
            }
        }
        else if (Pronouns.TryParse(argument, out var pronouns))
        {
            if (pronounsList.Remove(pronouns))
            {
                responseMarkdownV2 = $@"🚮 Successfully removed *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* from your list of pronouns\!";
            }
            else
            {
                responseMarkdownV2 = $@"Not found: *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}*";
            }
        }
        else
        {
            responseMarkdownV2 = $@"Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. Please follow the same format requirements as `/add\_pronouns`\.";
        }

        return botClient.SendTextMessageAsync(message.Chat.Id,
                                              responseMarkdownV2,
                                              ParseMode.MarkdownV2,
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);
    }

    public static Task GetPronounsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        long userId;
        long groupId = 0L;
        if (message.Chat.Type != ChatType.Private)
        {
            groupId = message.Chat.Id;
        }

        // query someone else
        if (message.ReplyToMessage is Message targetMessage)
        {
            userId = targetMessage.From?.Id ?? 777000L;
            var firstname = targetMessage.From?.FirstName ?? string.Empty;
            var firstnameEscaped = ChatHelper.EscapeMarkdownV2Plaintext(firstname);
            var pronouns = data.GetPronounsToUse(userId, groupId);
            var responseMarkdownV2 = pronouns.Length switch
            {
                0 => $@"*{firstnameEscaped}* has not set any pronouns yet\. You may address *{firstnameEscaped}* by *{Pronouns.Neutral.ToSubjectObject()}*\.",
                _ => $@"You may address *{firstnameEscaped}* by {string.Join(", ", pronouns.Select(x => $"*{ChatHelper.EscapeMarkdownV2Plaintext(x.ToSubjectObject())}*"))}\.",
            };
            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  responseMarkdownV2,
                                                  ParseMode.MarkdownV2,
                                                  replyToMessageId: message.MessageId,
                                                  cancellationToken: cancellationToken);
        }

        // self query
        userId = message.From?.Id ?? 777000L;
        var (allPronouns, defaultPronouns, preferredPronouns) = data.GetPronounsInfo(userId, groupId);
        var responseSB = new StringBuilder();

        responseSB.AppendLine($"Pronouns: {allPronouns.Length}");
        foreach (var p in allPronouns)
        {
            responseSB.AppendLine($"- {p}");
        }

        responseSB.AppendLine();

        responseSB.AppendLine($"Default pronoun: {(defaultPronouns is null ? "Not set" : defaultPronouns)}");
        responseSB.AppendLine($"Preferred pronoun in this chat: {(preferredPronouns is null ? "Not set" : preferredPronouns)}");

        return botClient.SendTextMessageAsync(message.Chat.Id,
                                              responseSB.ToString(),
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);
    }

    public static Task SetDefaultPronounsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        var userId = message.From?.Id ?? 777000L;
        var userData = data.GetOrCreateUserData(userId);

        if (string.IsNullOrEmpty(argument)) // unset
        {
            if (userData.DefaultPronouns is not null)
            {
                var pronouns = userData.DefaultPronouns;
                userData.DefaultPronouns = null;
                responseMarkdownV2 = $@"➖ Successfully unset your previous default pronouns *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}*\!";
            }
            else
            {
                responseMarkdownV2 = @"You don't already have default pronouns\. Specify one to set as default pronouns\.";
            }
        }
        else if (Pronouns.TryParse(argument, out var pronouns)) // set
        {
            if (userData.PronounList.Contains(pronouns))
            {
                userData.DefaultPronouns = pronouns;
                responseMarkdownV2 = $@"🚮 Successfully set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your default pronouns\!";
            }
            else
            {
                userData.DefaultPronouns = pronouns;
                userData.PronounList.Add(pronouns);
                responseMarkdownV2 = $@"🚮 Successfully added and set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your default pronouns\!";
            }
        }
        else
        {
            responseMarkdownV2 = $@"Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. Please follow the same format requirements as `/add\_pronouns`\.";
        }

        return botClient.SendTextMessageAsync(message.Chat.Id,
                                              responseMarkdownV2,
                                              ParseMode.MarkdownV2,
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);
    }

    public static Task SetPreferredPronounsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        var userId = message.From?.Id ?? 777000L;
        var userData = data.GetOrCreateUserData(userId);

        var memberData = message.Chat.Type switch
        {
            ChatType.Private => null,
            _ => data.GetOrCreateGroupData(message.Chat.Id).GetOrCreateUserData(userId),
        };
        var preferredPronouns = message.Chat.Type switch
        {
            ChatType.Private => userData.PreferredPronouns,
            _ => memberData!.PreferredPronouns, // null forgiving reason: safeguarded by previous lines.
        };
        Action<Pronouns?> setPreferredPronouns = message.Chat.Type switch
        {
            ChatType.Private => x => userData.PreferredPronouns = x,
            _ => x => memberData!.PreferredPronouns = x, // null forgiving reason: safeguarded by previous lines.
        };

        if (string.IsNullOrEmpty(argument)) // unset
        {
            if (preferredPronouns is not null)
            {
                setPreferredPronouns(null);
                responseMarkdownV2 = $@"➖ Successfully unset your previous preferred pronouns *{ChatHelper.EscapeMarkdownV2Plaintext(preferredPronouns.ToString())}* in this chat\!";
            }
            else
            {
                responseMarkdownV2 = @"You don't already have preferred pronouns in this chat\. Specify one to set as preferred pronouns in this chat\.";
            }
        }
        else if (Pronouns.TryParse(argument, out var pronouns)) // set
        {
            if (userData.PronounList.Contains(pronouns))
            {
                setPreferredPronouns(pronouns);
                responseMarkdownV2 = $@"🚮 Successfully set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your preferred pronouns in this chat\!";
            }
            else
            {
                setPreferredPronouns(pronouns);
                userData.PronounList.Add(pronouns);
                responseMarkdownV2 = $@"🚮 Successfully added and set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your preferred pronouns in this chat\!";
            }
        }
        else
        {
            responseMarkdownV2 = $@"Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. Please follow the same format requirements as `/add\_pronouns`\.";
        }

        return botClient.SendTextMessageAsync(message.Chat.Id,
                                              responseMarkdownV2,
                                              ParseMode.MarkdownV2,
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);
    }
}
