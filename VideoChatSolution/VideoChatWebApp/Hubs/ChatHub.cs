namespace VideoChatWebApp.Hubs
{
    using VideoChatWebApp.Models;

    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }

        public void ImageMessage(string username, ImageMessage file)
        {
            Clients.All.SendAsync("imageMessage", username, file);
        }
    }
}