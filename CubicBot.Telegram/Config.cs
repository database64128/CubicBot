using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram;

public sealed class Config
{
    /// <summary>
    /// Defines the default config version
    /// used by this version of the app.
    /// </summary>
    public const int DefaultVersion = 1;

    /// <summary>
    /// Gets or sets the config version number.
    /// </summary>
    public int Version { get; set; } = DefaultVersion;

    /// <summary>
    /// Gets or sets the Telegram bot token.
    /// </summary>
    public string BotToken { get; set; } = "";

    /// <summary>
    /// Gets or sets whether to enable commands.
    /// Disabling this overrides the commands config.
    /// </summary>
    public bool EnableCommands { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to enable stats.
    /// Disabling this overrides the stats config.
    /// </summary>
    public bool EnableStats { get; set; } = true;

    /// <summary>
    /// Gets or sets the config object for commands.
    /// </summary>
    public CommandsConfig Commands { get; set; } = new();

    /// <summary>
    /// Gets or sets the config object for stats.
    /// </summary>
    public StatsConfig Stats { get; set; } = new();

    /// <summary>
    /// Loads config from config.json.
    /// </summary>
    /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
    /// <returns>The <see cref="Config"/> object.</returns>
    public static async Task<Config> LoadAsync(CancellationToken cancellationToken = default)
    {
        var config = await FileHelper.LoadFromJsonFileAsync("config.json", ConfigJsonSerializerContext.Default.Config, cancellationToken);
        if (config.Version != DefaultVersion)
        {
            config.UpdateConfigVersion();
            await config.SaveAsync(cancellationToken);
        }
        return config;
    }

    /// <summary>
    /// Saves config to config.json.
    /// </summary>
    /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task SaveAsync(CancellationToken cancellationToken = default)
        => FileHelper.SaveToJsonFileAsync("config.json", this, ConfigJsonSerializerContext.Default.Config, cancellationToken);

    /// <summary>
    /// Updates config to the latest version.
    /// </summary>
    public void UpdateConfigVersion()
    {
        switch (Version)
        {
            case 0:
                Version++;
                goto case 1;
            case DefaultVersion:
                return;
            default:
                throw new NotSupportedException($"Config version {Version} is not supported.");
        }
    }
}
