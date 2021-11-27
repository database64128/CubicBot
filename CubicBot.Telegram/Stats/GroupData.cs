using System.Collections.Generic;

namespace CubicBot.Telegram.Stats
{
    public class GroupData
    {
        public ulong MessagesProcessed { get; set; }
        public ulong CommandsHandled { get; set; }

        public Dictionary<long, UserData> Members { get; set; } = new();

        /// <summary>
        /// Gets the member's stats object.
        /// If the object doesn't already exist,
        /// a new instance is created and added to storage.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>The member's stats object.</returns>
        public UserData GetOrCreateUserData(long id)
        {
            if (Members.TryGetValue(id, out var userData))
            {
                return userData;
            }
            else
            {
                var newUserData = new UserData();
                Members.Add(id, newUserData);
                return newUserData;
            }
        }
    }
}
