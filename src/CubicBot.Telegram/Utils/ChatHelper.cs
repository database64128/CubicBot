using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CubicBot.Telegram.Utils;

public static partial class ChatHelper
{
    /// <summary>
    /// Sends a text message.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendMessage"/>
    public static Task<Message> SendTextMessageAsync(
        this MessageContext messageContext,
        string text,
        ParseMode parseMode = default,
        ReplyParameters? replyParameters = default,
        ReplyMarkup? replyMarkup = default,
        LinkPreviewOptions? linkPreviewOptions = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? entities = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
        => messageContext.BotClient.SendMessage(
            messageContext.Message.Chat.Id,
            text,
            parseMode,
            replyParameters,
            replyMarkup,
            linkPreviewOptions,
            messageThreadId,
            entities,
            disableNotification,
            protectContent,
            messageEffectId,
            businessConnectionId,
            allowPaidBroadcast,
            directMessagesTopicId,
            suggestedPostParameters,
            cancellationToken);

    /// <summary>
    /// Replies with a text message.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="SendTextMessageAsync"/>
    public static Task<Message> ReplyWithTextMessageAsync(
        this MessageContext messageContext,
        string text,
        ParseMode parseMode = default,
        ReplyMarkup? replyMarkup = default,
        LinkPreviewOptions? linkPreviewOptions = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? entities = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
        => messageContext.SendTextMessageAsync(
            text,
            parseMode,
            messageContext.Message.Id,
            replyMarkup,
            linkPreviewOptions,
            messageThreadId,
            entities,
            disableNotification,
            protectContent,
            messageEffectId,
            businessConnectionId,
            allowPaidBroadcast,
            directMessagesTopicId,
            suggestedPostParameters,
            cancellationToken);

    /// <summary>
    /// Replies to the grandparent message with a text message.
    /// </summary>
    /// <remarks>
    /// If there is no grandparent message, the message is sent as a new message.
    /// </remarks>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="ReplyWithTextMessageAsync"/>
    public static Task<Message> ReplyToGrandparentWithTextMessageAsync(
        this MessageContext messageContext,
        string text,
        ParseMode parseMode = default,
        ReplyMarkup? replyMarkup = default,
        LinkPreviewOptions? linkPreviewOptions = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? entities = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
        => messageContext.SendTextMessageAsync(
            text,
            parseMode,
            messageContext.Message.ReplyToMessage?.Id,
            replyMarkup,
            linkPreviewOptions,
            messageThreadId,
            entities,
            disableNotification,
            protectContent,
            messageEffectId,
            businessConnectionId,
            allowPaidBroadcast,
            directMessagesTopicId,
            suggestedPostParameters,
            cancellationToken);

    /// <summary>
    /// Sends a file.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDocument"/>
    public static Task<Message> SendDocumentAsync(
        this MessageContext messageContext,
        InputFile document,
        string? caption = default,
        ParseMode parseMode = default,
        ReplyParameters? replyParameters = default,
        ReplyMarkup? replyMarkup = default,
        InputFile? thumbnail = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool disableContentTypeDetection = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
        => messageContext.BotClient.SendDocument(
            messageContext.Message.Chat.Id,
            document,
            caption,
            parseMode,
            replyParameters,
            replyMarkup,
            thumbnail,
            messageThreadId,
            captionEntities,
            disableContentTypeDetection,
            disableNotification,
            protectContent,
            messageEffectId,
            businessConnectionId,
            allowPaidBroadcast,
            directMessagesTopicId,
            suggestedPostParameters,
            cancellationToken);

    /// <summary>
    /// Edits a text message.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="TelegramBotClientExtensions.EditMessageText"/>
    public static Task<Message> EditMessageTextWithRetryAsync(
        this MessageContext messageContext,
        int messageId,
        string text,
        ParseMode parseMode = default,
        InlineKeyboardMarkup? replyMarkup = default,
        LinkPreviewOptions? linkPreviewOptions = default,
        IEnumerable<MessageEntity>? entities = default,
        string? businessConnectionId = default,
        CancellationToken cancellationToken = default)
        => messageContext.BotClient.EditMessageText(
            messageContext.Message.Chat.Id,
            messageId,
            text,
            parseMode,
            replyMarkup,
            linkPreviewOptions,
            entities,
            businessConnectionId,
            cancellationToken);

    /// <summary>
    /// Sends an animated emoji.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDice"/>
    public static Task<Message> SendDiceWithRetryAsync(
        this MessageContext messageContext,
        string? emoji = default,
        ReplyParameters? replyParameters = default,
        ReplyMarkup? replyMarkup = default,
        int? messageThreadId = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
        => messageContext.BotClient.SendDice(
            messageContext.Message.Chat.Id,
            emoji,
            replyParameters,
            replyMarkup,
            messageThreadId,
            disableNotification,
            protectContent,
            messageEffectId,
            businessConnectionId,
            allowPaidBroadcast,
            directMessagesTopicId,
            suggestedPostParameters,
            cancellationToken);

    /// <summary>
    /// Sends a possibly long text message.
    /// Short messages are sent as text messages.
    /// Long messages are sent as text files.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <inheritdoc cref="SendTextMessageAsync"/>
    public static Task<Message> SendPossiblyLongTextMessageAsync(
        this MessageContext messageContext,
        string text,
        ParseMode parseMode = default,
        ReplyParameters? replyParameters = default,
        ReplyMarkup? replyMarkup = default,
        LinkPreviewOptions? linkPreviewOptions = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? entities = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
        => text.Length switch
        {
            <= 4096 => messageContext.SendTextMessageAsync(
                text,
                parseMode,
                replyParameters,
                replyMarkup,
                linkPreviewOptions,
                messageThreadId,
                entities,
                disableNotification,
                protectContent,
                messageEffectId,
                businessConnectionId,
                allowPaidBroadcast,
                directMessagesTopicId,
                suggestedPostParameters,
                cancellationToken),
            _ => messageContext.SendTextFileFromStringWithRetryAsync(
                parseMode switch
                {
                    ParseMode.Markdown => "long-message.md",
                    ParseMode.Html => "long-message.html",
                    ParseMode.MarkdownV2 => "long-message.md",
                    _ => "long-message.txt",
                },
                text,
                default,
                parseMode,
                replyParameters,
                replyMarkup,
                default,
                messageThreadId,
                default,
                default,
                disableNotification,
                protectContent,
                messageEffectId,
                businessConnectionId,
                allowPaidBroadcast,
                directMessagesTopicId,
                suggestedPostParameters,
                cancellationToken)
        };

    /// <summary>
    /// Sends a string as a text file.
    /// </summary>
    /// <param name="messageContext">The message context.</param>
    /// <param name="filename">The filename of the text file.</param>
    /// <param name="text">The string to send.</param>
    /// <inheritdoc cref="SendDocumentAsync"/>
    public static async Task<Message> SendTextFileFromStringWithRetryAsync(
        this MessageContext messageContext,
        string filename,
        string text,
        string? caption = default,
        ParseMode parseMode = default,
        ReplyParameters? replyParameters = default,
        ReplyMarkup? replyMarkup = default,
        InputFile? thumbnail = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool disableContentTypeDetection = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        int? directMessagesTopicId = default,
        SuggestedPostParameters? suggestedPostParameters = default,
        CancellationToken cancellationToken = default)
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return await messageContext.SendDocumentAsync(
            InputFile.FromStream(stream, filename),
            caption,
            parseMode,
            replyParameters,
            replyMarkup,
            thumbnail,
            messageThreadId,
            captionEntities,
            disableContentTypeDetection,
            disableNotification,
            protectContent,
            messageEffectId,
            businessConnectionId,
            allowPaidBroadcast,
            directMessagesTopicId,
            suggestedPostParameters,
            cancellationToken);
    }

    [GeneratedRegex("[_*[\\]()~`>#+\\-=|{}.!]")]
    private static partial Regex EscapeMarkdownV2PlaintextRegex();

    [GeneratedRegex("[`\\\\]")]
    private static partial Regex EscapeMarkdownV2CodeBlockRegex();

    /// <summary>
    /// Escapes the plaintext per the MarkdownV2 requirements.
    /// This method does not handle markdown entities.
    /// </summary>
    /// <param name="plaintext">The plaintext to be escaped.</param>
    /// <returns>An escaped string.</returns>
    public static string EscapeMarkdownV2Plaintext(string plaintext)
        => EscapeMarkdownV2PlaintextRegex().Replace(plaintext, @"\$&");

    /// <summary>
    /// Escapes the code per the MarkdownV2 requirements.
    /// </summary>
    /// <param name="code">The code to be escaped.</param>
    /// <returns>The escaped code.</returns>
    public static string EscapeMarkdownV2CodeBlock(string code)
        => EscapeMarkdownV2CodeBlockRegex().Replace(code, @"\$&");

    /// <summary>
    /// Parses a text message into a command and an argument if applicable.
    /// </summary>
    /// <param name="text">The text message to process.</param>
    /// <param name="botUsername">The bot username. Does not start with '@'.</param>
    /// <returns>
    /// A ValueTuple of the parsed command and argument.
    /// Command is null if the text message is not a command to the bot.
    /// Argument can be null.
    /// </returns>
    public static (string? command, string? argument) ParseMessageIntoCommandAndArgument(ReadOnlySpan<char> text, string botUsername)
    {
        // Empty message
        if (text.IsEmpty)
            return (null, null);

        // Not a command
        if (text[0] != '/' || text.Length < 2)
            return (null, null);

        // Remove the leading '/'
        text = text[1..];

        // Split command and argument
        ReadOnlySpan<char> command, argument;
        var spacePos = text.IndexOf(' ');
        if (spacePos == -1)
        {
            command = text;
            argument = [];
        }
        else if (spacePos == text.Length - 1)
        {
            command = text[..spacePos];
            argument = [];
        }
        else
        {
            command = text[..spacePos];
            argument = text[(spacePos + 1)..];
        }

        // Verify and remove trailing '@bot' from command
        var atSignIndex = command.IndexOf('@');
        if (atSignIndex != -1)
        {
            if (atSignIndex != command.Length - 1)
            {
                var atUsername = command[(atSignIndex + 1)..];
                if (!atUsername.SequenceEqual(botUsername))
                {
                    return (null, null);
                }
            }

            command = command[..atSignIndex];
        }

        // Trim leading and trailing spaces from argument
        argument = argument.Trim();

        // Convert back to string
        string? commandString = null;
        string? argumentString = null;
        if (!command.IsEmpty)
        {
            commandString = command.ToString();

            if (!argument.IsEmpty)
            {
                argumentString = argument.ToString();
            }
        }

        return (commandString, argumentString);
    }

    /// <summary>
    /// Gets the message's sender ID.
    /// If the message was sent to a channel, Telegram's "default" ID (777000) is returned.
    /// </summary>
    /// <param name="message">A message.</param>
    /// <returns>The message's sender ID.</returns>
    public static long GetMessageSenderId(Message message) => message.From?.Id ?? 777000L;

    /// <summary>
    /// Gets a text message's text, or a picture/video's caption, otherwise returns an empty string.
    /// </summary>
    /// <param name="message">A message.</param>
    /// <returns>A non-null string.</returns>
    public static string GetMessageText(Message message) => message.Text ?? message.Caption ?? "";
}
