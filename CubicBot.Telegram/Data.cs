using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram;

public sealed class Data
{
    /// <summary>
    /// Defines the default data version
    /// used by this version of the app.
    /// </summary>
    public const int DefaultVersion = 2;

    /// <summary>
    /// Gets or sets the data version number.
    /// </summary>
    public int Version { get; set; } = DefaultVersion;

    /// <summary>
    /// Gets or sets the dictionary that stores user stats.
    /// Key is user ID.
    /// Value is user stats object.
    /// </summary>
    public Dictionary<long, UserData> Users { get; set; } = new();

    /// <summary>
    /// Gets or sets the dictionary that stores group stats.
    /// Key is group ID.
    /// Value is group stats object.
    /// </summary>
    public Dictionary<long, GroupData> Groups { get; set; } = new();

    /// <summary>
    /// Gets the user's stats object.
    /// If the object doesn't already exist,
    /// a new instance is created and added to storage.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>The user's stats object.</returns>
    public UserData GetOrCreateUserData(long id)
    {
        if (!Users.TryGetValue(id, out var userData))
        {
            userData = new();
            Users.Add(id, userData);
        }
        return userData;
    }

    /// <summary>
    /// Gets the group's stats object.
    /// If the object doesn't already exist,
    /// a new instance is created and added to storage.
    /// </summary>
    /// <param name="id">Group ID.</param>
    /// <returns>The group's stats object.</returns>
    public GroupData GetOrCreateGroupData(long id)
    {
        if (!Groups.TryGetValue(id, out var groupData))
        {
            groupData = new();
            Groups.Add(id, groupData);
        }
        return groupData;
    }

    /// <summary>
    /// Loads data from data.json.
    /// </summary>
    /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
    /// <returns>The <see cref="Data"/> object.</returns>
    public static async Task<Data> LoadAsync(CancellationToken cancellationToken = default)
    {
        var data = await FileHelper.LoadFromJsonFileAsync("data.json", DataJsonSerializerContext.Default.Data, cancellationToken);
        if (data.Version != DefaultVersion)
        {
            data.UpdateDataVersion();
            await data.SaveAsync(cancellationToken);
        }
        return data;
    }

    /// <summary>
    /// Saves data to data.json.
    /// </summary>
    /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task SaveAsync(CancellationToken cancellationToken = default)
        => FileHelper.SaveToJsonFileAsync("data.json", this, DataJsonSerializerContext.Default.Data, cancellationToken);

    /// <summary>
    /// Updates data to the latest version.
    /// </summary>
    public void UpdateDataVersion()
    {
        switch (Version)
        {
            case 0:
                Version++;
                goto case 1;

            case 1:
                foreach (var (_, groupData) in Groups)
                {
                    foreach (var (_, userData) in groupData.Members)
                    {
                        groupData.InterrogatedByOthers += userData.InterrogatedByOthers;
                        groupData.GrassGrown += userData.GrassGrown;
                        groupData.ParenthesesUnenclosed += userData.ParenthesesUnenclosed;
                    }
                }

                Version++;
                goto case 2;

            case DefaultVersion:
                return;

            default:
                throw new NotSupportedException($"Data version {Version} is not supported.");
        }
    }
}
