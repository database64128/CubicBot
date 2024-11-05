using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CubicBot.Telegram;

public sealed class BotService(ILogger<BotService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => RunBotAsync(stoppingToken);

    private async Task RunBotAsync(CancellationToken cancellationToken = default)
    {
        Config config;

        try
        {
            config = await Config.LoadAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load config", ex);
        }

        // Priority: commandline option > environment variable > config file
        var botToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        if (string.IsNullOrEmpty(botToken))
            botToken = config.BotToken;
        if (string.IsNullOrEmpty(botToken))
            throw new Exception("Please provide a bot token with environment variable `TELEGRAM_BOT_TOKEN`, or in the config file.");

        Data data;

        try
        {
            data = await Data.LoadAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load data", ex);
        }

        var saveDataTask = SaveDataHourlyAsync(data, cancellationToken);

        TelegramBotClientOptions options = new(botToken)
        {
            RetryCount = 7,
        };
        using HttpClient httpClient = new();
        TelegramBotClient bot = new(options, httpClient);
        User me;

        while (true)
        {
            try
            {
                me = await bot.GetMe(cancellationToken);
                break;
            }
            catch (RequestException ex)
            {
                logger.LogWarning(ex, "Failed to get bot info, retrying in 30 seconds");
                await Task.Delay(30000, cancellationToken);
            }
        }

        if (string.IsNullOrEmpty(me.Username))
            throw new Exception("Bot username is null or empty.");

        var updateHandler = new UpdateHandler(me.Username, config, data, logger);

        while (true)
        {
            try
            {
                await updateHandler.RegisterCommandsAsync(bot, cancellationToken);
                break;
            }
            catch (RequestException ex)
            {
                logger.LogWarning(ex, "Failed to register commands, retrying in 30 seconds");
                await Task.Delay(30000, cancellationToken);
            }
        }

        logger.LogInformation("Started Telegram bot: @{BotUsername} ({BotId})", me.Username, me.Id);

        var updateReceiver = new QueuedUpdateReceiver(bot, null, updateHandler.HandleErrorAsync);
        await updateHandler.HandleUpdateStreamAsync(bot, updateReceiver, cancellationToken);

        await saveDataTask;
    }

    private async Task SaveDataHourlyAsync(Data data, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromHours(1.0), cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }

            try
            {
                await data.SaveAsync(CancellationToken.None);
                logger.LogDebug("Saved data");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save data");
            }
        }
    }
}
