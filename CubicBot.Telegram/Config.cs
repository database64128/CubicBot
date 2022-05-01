using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram
{
    public class Config
    {
        /// Gets the default config version
        /// used by this version of the app.
        /// </summary>
        public static int DefaultVersion => 1;

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
        /// <returns>
        /// A ValueTuple containing a <see cref="Config"/> object and an optional error message.
        /// </returns>
        public static async Task<(Config config, string? errMsg)> LoadConfigAsync(CancellationToken cancellationToken = default)
        {
            var (config, errMsg) = await FileHelper.LoadJsonAsync("config.json", ConfigJsonSerializerContext.Default.Config, cancellationToken);
            if (errMsg is null && config.Version != DefaultVersion)
            {
                config.UpdateConfig();
                errMsg = await SaveConfigAsync(config, cancellationToken);
            }
            return (config, errMsg);
        }

        /// <summary>
        /// Saves config to config.json.
        /// </summary>
        /// <param name="config">The <see cref="Config"/> object to save.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
        /// <returns>
        /// An optional error message.
        /// Null if no errors occurred.
        /// </returns>
        public static Task<string?> SaveConfigAsync(Config config, CancellationToken cancellationToken = default)
            => FileHelper.SaveJsonAsync("config.json", config, ConfigJsonSerializerContext.Default.Config, false, false, cancellationToken);

        /// <summary>
        /// Updates the current object to the latest version.
        /// </summary>
        public void UpdateConfig()
        {
            switch (Version)
            {
                case 0: // nothing to do
                    Version++;
                    goto default; // go to the next update path
                default:
                    break;
            }
        }
    }
}
