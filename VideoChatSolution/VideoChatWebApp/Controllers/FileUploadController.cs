using System.IO;
using System.Threading.Tasks;

using VideoChatWebApp.Hubs;
using VideoChatWebApp.Models;
using ServiceLayer.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace VideoChatWebApp.Controllers
{
    [Authorize]
    [Route("api/FileUpload")]
    public class FileUploadController : Controller
    {
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IAuthManager authManager;

        public FileUploadController(IHubContext<ChatHub> hubContext, IAuthManager authManager)
        {
            this.hubContext = hubContext;
            this.authManager = authManager;
        }

        [Route("files")]
        [HttpPost("content/upload-image")]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            IFormFile fileFromRequest = null;

            if (file == null)
            {
                fileFromRequest = HttpContext?.Request?.Form?.Files?[0] ?? null;
            }

            if (file == null && fileFromRequest == null)
            {
                return BadRequest();
            }

            var user = await this.authManager.GetUserAsync(HttpContext.User);

            using (var memoryStream = new MemoryStream())
            {
                await fileFromRequest.CopyToAsync(memoryStream);

                var imageMessage = new ImageMessage
                {
                    ImageHeaders = "data:" + fileFromRequest.ContentType + ";base64,",
                    ImageBinary = memoryStream.ToArray()
                };

                if (hubContext.Clients != null) //TODO - Remove this ugly hack when FileUploadControllerTests is mocking the SignalR ChatHub.cs
                {
                    await hubContext.Clients?.All?.SendAsync("imageMessage", user.UserName, imageMessage);
                }
            }

            return Ok();
        }
    }
}
