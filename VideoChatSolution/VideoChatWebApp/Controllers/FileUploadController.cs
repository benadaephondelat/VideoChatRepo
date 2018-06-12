using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoChatWebApp.Hubs;

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
            var user = await this.authManager.GetUserAsync(HttpContext.User);

            IFormFile fileFromRequest = HttpContext.Request.Form.Files[0];

            using (var memoryStream = new MemoryStream())
            {
                await fileFromRequest.CopyToAsync(memoryStream);

                var imageMessage = new ImageMessage
                {
                    ImageHeaders = "data:" + fileFromRequest.ContentType + ";base64,",
                    ImageBinary = memoryStream.ToArray()
                };

                await hubContext.Clients.All.SendAsync("imageMessage", user.UserName, imageMessage);
            }

            return Ok();
        }
    }
}
