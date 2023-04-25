using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace CubicBot.Telegram.CLI;

public static class BotRunner
{
    public static async Task<int> RunBotAsync(string? botToken, CancellationToken cancellationToken = default)
    {
        Config config;

        try
        {
            config = await Config.LoadAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load config: {ex.Message}");
            return 1;
        }

        // Priority: commandline option > environment variable > config file
        if (string.IsNullOrEmpty(botToken))
            botToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        if (string.IsNullOrEmpty(botToken))
            botToken = config.BotToken;
        if (string.IsNullOrEmpty(botToken))
        {
            Console.WriteLine("Please provide a bot token with command line option `--bot-token`, environment variable `TELEGRAM_BOT_TOKEN`, or in the config file.");
            return 1;
        }

        Data data;

        try
        {
            data = await Data.LoadAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load data: {ex.Message}");
            return 1;
        }

        var saveDataTask = SaveDataHourlyAsync(data, cancellationToken);

        try
        {
            using var httpClient = new HttpClient();
            var bot = new TelegramBotClient(botToken, httpClient);
            var me = await bot.GetMeAsync(cancellationToken);
            if (me.CanReadAllGroupMessages is not true)
                config.EnableStats = false;
            if (string.IsNullOrEmpty(me.Username))
                throw new Exception("Bot username is null or empty.");

            var updateHandler = new UpdateHandler(me.Username, config, data);
            await bot.SetMyCommandsAsync(updateHandler.Commands, null, null, cancellationToken);
            Console.WriteLine($"Registered {updateHandler.Commands.Count} bot commands.");
            Console.WriteLine($"Started Telegram bot: @{me.Username} ({me.Id}).");

            var updateReceiver = new QueuedUpdateReceiver(bot, null, UpdateHandler.HandleErrorAsync);
            await updateHandler.HandleUpdateStreamAsync(bot, updateReceiver, cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start Telegram bot: {ex.Message}");
            return 1;
        }

        await saveDataTask;
        return 0;
    }

    private static async Task SaveDataHourlyAsync(Data data, CancellationToken cancellationToken = default)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save data: {ex.Message}");
            }
        }
    }
}
