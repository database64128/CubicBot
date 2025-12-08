using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace CubicBot.Telegram;

public partial class BotService : IHostedService
{
    private readonly ILogger<BotService> _logger;
    private readonly HttpClient _httpClient;
    private readonly ChannelReader<Update> _updateReader;

    public BotService(ILogger<BotService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        Channel<Update> updateChannel = Channel.CreateBounded<Update>(new BoundedChannelOptions(64)
        {
            SingleReader = true,
        });
        _updateReader = updateChannel.Reader;
        UpdateWriter = updateChannel.Writer;
    }

    public ChannelWriter<Update> UpdateWriter { get; }

    protected CancellationTokenSource? Cts { get; private set; }
    private Task? _saveDataTask;
    private Task? _handleUpdatesTask;

    public virtual Task StartAsync(CancellationToken cancellationToken) => StartBotAsync(cancellationToken);

    protected async Task<(TelegramBotClient, CancellationTokenSource)> StartBotAsync(CancellationToken cancellationToken = default)
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

        TelegramBotClientOptions options = new(botToken)
        {
            RetryCount = 7,
        };
        TelegramBotClient bot = new(options, _httpClient);
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
                _logger.LogWarning(ex, "Failed to get bot info, retrying in 30 seconds");
                await Task.Delay(s_delayOnError, cancellationToken);
            }
        }

        if (string.IsNullOrEmpty(me.Username))
            throw new Exception("Bot username is null or empty.");

        var updateHandler = new UpdateHandler(me.Username, config, data, _logger, bot);

        while (true)
        {
            try
            {
                await updateHandler.RegisterCommandsAsync(cancellationToken);
                break;
            }
            catch (RequestException ex)
            {
                _logger.LogWarning(ex, "Failed to register commands, retrying in 30 seconds");
                await Task.Delay(s_delayOnError, cancellationToken);
            }
        }

        LogStartedBot(me.Username, me.Id);

        Cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _saveDataTask = SaveDataHourlyAsync(data, Cts.Token);
        _handleUpdatesTask = updateHandler.RunAsync(_updateReader, Cts.Token);

        return (bot, Cts);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Started Telegram bot: @{BotUsername} ({BotId})")]
    private partial void LogStartedBot(string botUsername, long botId);

    private static readonly TimeSpan s_delayOnError = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan s_saveDataInterval = TimeSpan.FromHours(1);

    private async Task SaveDataHourlyAsync(Data data, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(s_saveDataInterval, cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }

            try
            {
                await data.SaveAsync(CancellationToken.None);
                _logger.LogDebug("Saved data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save data");
            }
        }
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Cts is null)
        {
            return;
        }

        try
        {
            Cts.Cancel();
        }
        finally
        {
            if (_saveDataTask is not null)
            {
                await _saveDataTask.WaitAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            }

            if (_handleUpdatesTask is not null)
            {
                await _handleUpdatesTask.WaitAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            }
        }
    }
}
