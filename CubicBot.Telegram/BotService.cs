using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace CubicBot.Telegram;

public sealed class BotService(ILogger<BotService> logger) : IHostedService
{
    private readonly CancellationTokenSource _cts = new();
    private Task _botTask = Task.CompletedTask;

    public Task StartAsync(CancellationToken _ = default)
    {
        _botTask = RunBotAsync(null, _cts.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken _ = default)
    {
        _cts.Cancel();
        return _botTask;
    }

    public async Task<int> RunBotAsync(string? botToken, CancellationToken cancellationToken = default)
    {
        Config config;

        try
        {
            config = await Config.LoadAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Failed to load config");
            return 1;
        }

        // Priority: commandline option > environment variable > config file
        if (string.IsNullOrEmpty(botToken))
            botToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        if (string.IsNullOrEmpty(botToken))
            botToken = config.BotToken;
        if (string.IsNullOrEmpty(botToken))
        {
            logger.LogCritical("Please provide a bot token with command line option `--bot-token`, environment variable `TELEGRAM_BOT_TOKEN`, or in the config file.");
            return 1;
        }

        Data data;

        try
        {
            data = await Data.LoadAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Failed to load data");
            return 1;
        }

        var saveDataTask = SaveDataHourlyAsync(data, cancellationToken);

        try
        {
            using var httpClient = new HttpClient();
            var bot = new TelegramBotClient(botToken, httpClient);
            var me = await bot.GetMeAsync(cancellationToken);

            if (string.IsNullOrEmpty(me.Username))
                throw new Exception("Bot username is null or empty.");

            var updateHandler = new UpdateHandler(me.Username, config, data, logger);
            await updateHandler.RegisterCommandsAsync(bot, cancellationToken);

            logger.LogInformation("Started Telegram bot: @{BotUsername} ({BotId})", me.Username, me.Id);

            var updateReceiver = new QueuedUpdateReceiver(bot, null, updateHandler.HandleErrorAsync);
            await updateHandler.HandleUpdateStreamAsync(bot, updateReceiver, cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to start Telegram bot");
            return 1;
        }

        await saveDataTask;
        return 0;
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
