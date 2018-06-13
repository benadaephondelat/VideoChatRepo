namespace VideoChatWebAppTests
{
    using System;
    using System.Threading.Tasks;

    using Models;
    using ServiceLayer.Interfaces;
    using TestMakerFreeWebApp.Controllers;
    using TestMakerFreeWebApp.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using Moq;

    public class UserControllerTests
    {
        private Mock<IUserService> userServiceMock;
        private UserController userController;

        [Fact]
        public async Task Add_Method_Should_Return_JsonResult()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.userServiceMock.Setup(m => m.RegisterNewUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(Task.FromResult(MockedUser()));
            this.userController = new UserController(userServiceMock.Object);

            UserViewModel model = this.CreateUserViewModel();

            var result = await this.userController.Add(model);

            this.Annihilate();

            Assert.IsType<JsonResult>(result);
        }

        private UserViewModel CreateUserViewModel()
        {
            UserViewModel model = new UserViewModel();
            model.DisplayName = "displayName";
            model.Email = "email@yahoo.com";
            model.Password = "password123*&";
            model.UserName = "Username";
            return model;
        }

        private ApplicationUser MockedUser()
        {
            ApplicationUser user = new ApplicationUser();

            user.Id = "mocked-user-id";
            user.UserName = "mocked-username";
            user.Email = "mocked@yahoo.com";
            user.PasswordHash = "mockedpassword";

            return user;
        }

        private void Annihilate()
        {
            this.userController = null;
            this.userServiceMock = null;
        }
    }
}
