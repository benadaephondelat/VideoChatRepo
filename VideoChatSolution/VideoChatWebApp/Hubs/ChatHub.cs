namespace VideoChatWebApp.Hubs
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

    public class ImageMessage
    {
        public byte[] ImageBinary { get; set; }
        public string ImageHeaders { get; set; }
    }

    public class FileDescriptionShort
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IFormFile> File { get; set; }
    }
}