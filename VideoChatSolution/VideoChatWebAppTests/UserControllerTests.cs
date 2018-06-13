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
    using VideoChatWebApp.Infrastrucure.Services;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    public class UserControllerTests
    {
        private Mock<IUserService> userServiceMock;
        private Mock<IAuthManager> authManagerMock;
        private UserController userController;

        [Fact]
        public async Task Add_Method_Should_Return_JsonResult()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.userServiceMock.Setup(m => m.RegisterNewUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(Task.FromResult(MockedUser()));
            this.authManagerMock = new Mock<IAuthManager>();
            this.userController = new UserController(userServiceMock.Object, authManagerMock.Object);

            UserViewModel model = this.CreateUserViewModel();

            var result = await this.userController.Add(model);

            this.Annihilate();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task Add_Method_Should_Return_400_OK_StatusCode_If_UserViewModel_Is_Null()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.authManagerMock = new Mock<IAuthManager>();
            this.userController = new UserController(userServiceMock.Object, authManagerMock.Object);

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
            this.authManagerMock = new Mock<IAuthManager>();
            this.userController = new UserController(userServiceMock.Object, authManagerMock.Object);

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
            this.authManagerMock = new Mock<IAuthManager>();
            this.userController = new UserController(userServiceMock.Object, authManagerMock.Object);

            UserViewModel model = this.CreateUserViewModel();

            var result = await this.userController.Add(model);

            this.Annihilate();

            var badRequest = result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task GetUsers_Should_Return_JsonResult()
        {
            this.Arrange();

            CurrentlyLoggedInUsersService.AddNewEntry("user", "user-id");
            CurrentlyLoggedInUsersService.AddNewEntry("second-user", "second-user-id");

            var result = await this.userController.GetUsers();

            this.Annihilate();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task GetUsers_Should_Return_All_Users_Except_The_Current_User()
        {
            this.Arrange();

            CurrentlyLoggedInUsersService.AddNewEntry("username", "user-id");
            CurrentlyLoggedInUsersService.AddNewEntry("second-user", "second-user-id");

            var actionResult = await this.userController.GetUsers();

            this.Annihilate();

            JsonResult result = actionResult as JsonResult;

            List<string> usernames = result.Value as List<string>;

            Assert.Single(usernames);
            Assert.Equal("second-user", usernames.ElementAt(0));
        }

        private void Arrange()
        {
            this.authManagerMock = new Mock<IAuthManager>();
            var mockedUser = this.MockedUser();
            this.authManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(mockedUser));

            var username = mockedUser.UserName;
            var identity = new GenericIdentity(username, "");

            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(mockPrincipal.Object);

            this.userServiceMock = new Mock<IUserService>();
            this.userController = new UserController(userServiceMock.Object, authManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
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
            user.UserName = "username";
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
