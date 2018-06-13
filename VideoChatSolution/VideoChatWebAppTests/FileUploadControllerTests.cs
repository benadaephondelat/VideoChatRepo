namespace VideoChatWebAppTests
{
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Security.Claims;

    using Models;
    using ServiceLayer.Interfaces;
    using VideoChatWebApp.Controllers;
    using VideoChatWebApp.Hubs;

    using Moq;
    using Xunit;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public class FileUploadControllerTests
    {
        private Mock<IHubContext<ChatHub>> chatHubMock;
        private Mock<IAuthManager> authManagerMock;
        private FileUploadController fileUploadController;

        [Fact]
        public async Task UploadFiles_Should_Return_Bad_Request_If_There_Is_No_File_In_The_HttpContext_Request_Form_Files()
        {
            this.chatHubMock = new Mock<IHubContext<ChatHub>>();

            this.authManagerMock = new Mock<IAuthManager>();

            this.fileUploadController = new FileUploadController(this.chatHubMock.Object, this.authManagerMock.Object);

            var result = await this.fileUploadController.UploadFiles(GetNullFormFile());

            this.Annihilate();

            var badRequestResult = result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task UploadFiles_Should_Return_200_Ok_If_There_Is_A_File_In_The_HttpContext_Request_Forms_Files()
        {
            this.chatHubMock = new Mock<IHubContext<ChatHub>>();

            this.authManagerMock = new Mock<IAuthManager>();

            var mockedUser = this.MockedUser();

            this.authManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(mockedUser));

            this.fileUploadController = new FileUploadController(this.chatHubMock.Object, this.authManagerMock.Object)
            {
                ControllerContext = RequestWithFile()
            };

            var result = await this.fileUploadController.UploadFiles(GetNullFormFile());

            this.Annihilate();

            var okRequestResult = result.Should().BeOfType<OkResult>();
        }

        private ControllerContext RequestWithFile()
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");

            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            file.Headers = new HeaderDictionary();

            file.Headers.Add("Content-Type", "content type");

            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());

            return new ControllerContext(actionContext);
        }

        private ApplicationUser MockedUser()
        {
            ApplicationUser user = new ApplicationUser();

            user.Id = "mocked-user-id";
            user.UserName = "username";
            user.Email = "mocked@yahoo.com";
            user.PasswordHash = "mockedpassword";

            return user;
        }

        private IFormFile GetNullFormFile()
        {
            return null;
        }

        private void Annihilate()
        {
            this.chatHubMock = null;
            this.authManagerMock = null;
            this.fileUploadController = null;
        }
    }
}