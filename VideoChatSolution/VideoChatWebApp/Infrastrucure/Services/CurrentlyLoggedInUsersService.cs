namespace VideoChatWebApp.Infrastrucure.Services
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Thread safe singleton
    /// <see cref="http://csharpindepth.com/Articles/General/Singleton.aspx"/>
    /// </summary>
    public sealed class CurrentlyLoggedInUsersSingleton
    {
        private static readonly CurrentlyLoggedInUsersSingleton instance = new CurrentlyLoggedInUsersSingleton();

        private static ConcurrentDictionary<string, string> dictionary;

        static CurrentlyLoggedInUsersSingleton()
        {
            
        }

        private CurrentlyLoggedInUsersSingleton()
        {
            dictionary = new ConcurrentDictionary<string, string>();
        }

        public static CurrentlyLoggedInUsersSingleton Instance
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
