namespace VideoChatWebApp.Hubs.Interfaces
{
    using System.Threading.Tasks;

    public interface IHubClient
    {
        //Task SendToAll(string methodName, string username);

        void AddUserToChatRoom(string username);

        void RemoveUserFromChatRoom(string username);
    }

    public enum MethodNames
    {
        AddUserToChatRoom,
        RemoveUserFromChatRoom
    }
}