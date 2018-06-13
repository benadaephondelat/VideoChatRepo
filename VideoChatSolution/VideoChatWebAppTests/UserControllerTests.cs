namespace VideoChatWebAppTests
{
    using System;
    using System.Threading.Tasks;

    using Models;
    using ServiceLayer.Interfaces;
    using TestMakerFreeWebApp.Controllers;
    using TestMakerFreeWebApp.ViewModels;
    using Common.CustomExceptions.UserExceptions;

    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using Moq;
    using FluentAssertions;

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

        [Fact]
        public async Task Add_Method_Should_Return_400_OK_StatusCode_If_UserViewModel_Is_Null()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.userController = new UserController(userServiceMock.Object);

            UserViewModel model = this.CreateNullUserViewModel();

            var result = await this.userController.Add(model);

            this.Annihilate();

            var badRequest = result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Add_Method_Should_Return_Conflict_StatusCode_If_ServiceLayer_Throws_UserAlreadyExists_Exception()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.userServiceMock.Setup(m => m.RegisterNewUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                .Throws<UserAlreadyExistsException>();

            this.userController = new UserController(userServiceMock.Object);

            UserViewModel model = this.CreateUserViewModel();

            var result = await this.userController.Add(model);

            this.Annihilate();

            var badRequest = result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task Add_Method_Should_Return_Conflict_StatusCode_If_ServiceLayer_Throws_UserPasswordValidationException_Exception()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.userServiceMock.Setup(m => m.RegisterNewUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                .Throws<UserPasswordValidationException>();

            this.userController = new UserController(userServiceMock.Object);

            UserViewModel model = this.CreateUserViewModel();

            var result = await this.userController.Add(model);

            this.Annihilate();

            var badRequest = result.Should().BeOfType<ConflictResult>();
        }

        private UserViewModel CreateNullUserViewModel()
        {
            return null;
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
