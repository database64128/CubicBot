using CubicBot.Telegram.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Data
{
    public class Data
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
        /// Gets or sets the number of processed messages.
        /// </summary>
        public ulong MessagesProcessed { get; set; }

        /// <summary>
        /// Gets or sets the number of handled commands.
        /// </summary>
        public ulong CommandsHandled { get; set; }

        /// <summary>
        /// Gets or sets the dictionary that stores user stats.
        /// Key is user ID.
        /// Value is user stat object.
        /// </summary>
        public Dictionary<long, UserData> Users { get; set; } = new();

        /// <summary>
        /// Gets or sets the dictionary that stores group stats.
        /// Key is group ID.
        /// Value is group stat object.
        /// </summary>
        public Dictionary<long, GroupData> Groups { get; set; } = new();

        /// <summary>
        /// Loads data from data.json.
        /// </summary>
        /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
        /// <returns>
        /// A ValueTuple containing a <see cref="Data"/> object and an optional error message.
        /// </returns>
        public static async Task<(Data data, string? errMsg)> LoadDataAsync(CancellationToken cancellationToken = default)
        {
            var (data, errMsg) = await FileHelper.LoadJsonAsync<Data>("data.json", FileHelper.commonJsonDeserializerOptions, cancellationToken);
            if (errMsg is null && data.Version != DefaultVersion)
            {
                data.UpdateConfig();
                errMsg = await SaveDataAsync(data, cancellationToken);
            }
            return (data, errMsg);
        }

        /// <summary>
        /// Saves data to data.json.
        /// </summary>
        /// <param name="data">The <see cref="Data"/> object to save.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
        /// <returns>
        /// An optional error message.
        /// Null if no errors occurred.
        /// </returns>
        public static Task<string?> SaveDataAsync(Data data, CancellationToken cancellationToken = default)
            => FileHelper.SaveJsonAsync("data.json", data, FileHelper.commonJsonSerializerOptions, false, false, cancellationToken);

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
