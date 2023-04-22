using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace CubicBot.Telegram.CLI
{
    public static class BotRunner
    {
        public static async Task<int> RunBotAsync(string? botToken, CancellationToken cancellationToken = default)
        {
            var (config, loadConfigErrMsg) = await Config.LoadConfigAsync(cancellationToken);
            if (loadConfigErrMsg is not null)
            {
                Console.WriteLine(loadConfigErrMsg);
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
                return -1;
            }

            var (data, loadDataErrMsg) = await Data.LoadDataAsync(cancellationToken);
            if (loadDataErrMsg is not null)
            {
                Console.WriteLine(loadDataErrMsg);
                return 1;
            }

            var saveDataTask = SaveDataHourlyAsync(data, cancellationToken);

            try
            {
                var bot = new TelegramBotClient(botToken);
                Console.WriteLine("Created Telegram bot instance with API token.");

                var me = await bot.GetMeAsync(cancellationToken);
                if (me.CanReadAllGroupMessages is not true)
                    config.EnableStats = false;
                if (string.IsNullOrEmpty(me.Username))
                    throw new Exception("Error: bot username is null or empty.");

                var updateHandler = new UpdateHandler(me.Username, config, data);
                await bot.SetMyCommandsAsync(updateHandler.Commands, null, null, cancellationToken);
                Console.WriteLine($"Registered {updateHandler.Commands.Count} bot commands.");
                Console.WriteLine($"Started Telegram bot: @{me.Username} ({me.Id}).");

                var updateReceiver = new QueuedUpdateReceiver(bot, null, UpdateHandler.HandleErrorAsync);
                await updateHandler.HandleUpdateStreamAsync(bot, updateReceiver, cancellationToken);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid access token: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"A network error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

                var saveDataErrMsg = await Data.SaveDataAsync(data, CancellationToken.None);
                if (saveDataErrMsg is not null)
                {
                    Console.WriteLine(saveDataErrMsg);
                }
            }
        }
    }
}
