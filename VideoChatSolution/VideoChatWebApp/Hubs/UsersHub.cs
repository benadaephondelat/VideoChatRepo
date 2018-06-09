namespace VideoChatWebApp.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using VideoChatWebApp.Hubs.Interfaces;

    public class UsersHub : Hub
    {
        public void SendToAll(string methodName, string username)
        {
            Clients.All.SendAsync(methodName.ToString(), username);
        }

        public void AddUserToChatRoom(string username)
        {
            Clients.All.SendAsync("addUserToChatRoom", username);
        }

        public void RemoveUserFromChatRoom(string username)
        {
            Clients.All.SendAsync("removeUserFromChatRoom", username);
        }
    }
}