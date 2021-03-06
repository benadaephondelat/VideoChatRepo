﻿namespace VideoChatWebApp.Hubs
{
    using Microsoft.AspNetCore.SignalR;

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

        public void JoinRoom(string currentUser, string otherUser)
        {
            Clients.All.SendAsync("joinRoom", currentUser, otherUser);
        }
    }
}