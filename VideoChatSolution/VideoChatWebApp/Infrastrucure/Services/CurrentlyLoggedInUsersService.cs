using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoChatWebApp.Infrastrucure.Services
{
    /// <summary>
    /// Thread safe singleton
    /// <see cref="http://csharpindepth.com/Articles/General/Singleton.aspx"/>
    /// </summary>
    public sealed class CurrentlyLoggedInUsersService
    {
        private static readonly CurrentlyLoggedInUsersService instance = new CurrentlyLoggedInUsersService();

        private static ConcurrentDictionary<string, string> dictionary;

        static CurrentlyLoggedInUsersService()
        {
            
        }

        private CurrentlyLoggedInUsersService()
        {
            dictionary = new ConcurrentDictionary<string, string>();
        }

        public static CurrentlyLoggedInUsersService Instance
        {
            get
            {
                return instance;
            }
        }

        public static bool AddNewEntry(string username, string userId)
        {
            bool isAddSuccess = dictionary.TryAdd(username, userId);

            return isAddSuccess;
        }

        public static bool RemoveEntryByKey(string username)
        {
            string previousValue = string.Empty;

            bool isRemoveEntryByKeySuccess = dictionary.TryRemove(username, out previousValue);

            return isRemoveEntryByKeySuccess;
        }

        public static IEnumerable<string> GetAllUsernames()
        {
            return dictionary.Keys.AsEnumerable();
        }
    }
}
