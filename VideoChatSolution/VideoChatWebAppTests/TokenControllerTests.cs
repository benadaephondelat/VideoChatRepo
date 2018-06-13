namespace VideoChatWebAppTests
{
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;

    using Models;
    using ServiceLayer.Interfaces;
    using TestMakerFreeWebApp.Controllers;
    using TestMakerFreeWebApp.ViewModels;
    using Common.CustomExceptions.UserExceptions;
    using VideoChatWebApp.Infrastrucure.Services;

    using Moq;
    using Xunit;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class TokenControllerTests
    {
        private Mock<ITokenService> tokenServiceMock;
        private Mock<IConfiguration> configurationMock;
        private IConfiguration configuration;
        private TokenController tokenController;

        [Fact]
        public async Task Auth_Should_Return_BadRequest_If_TokenRequestViewModel_Is_Null()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            IActionResult result = await this.tokenController.Auth(this.CreateNullTokenRequestModel());

            this.Annihilate();

            var badRequestResult = result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Auth_Should_Return_UnauthorizedResult_If_TokenRequestViewModel_GrantType_Is_Not_password_Or_refresh_token()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            var model = this.CreateTokenRequestModelWithPasswordGrantType();
            model.grant_type = string.Empty;

            IActionResult result = await this.tokenController.Auth(model);

            this.Annihilate();

            var unauthorizedResult = result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Auth_Should_Return_UnauthorizedResult_If_GrantType_Is_password_And_TokenService_Throws_UserNotFoundException()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.CreateUserRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .Throws<UserNotFoundException>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            IActionResult result = await this.tokenController.Auth(this.CreateTokenRequestModelWithPasswordGrantType());

            this.Annihilate();

            var unauthorizedResult = result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Auth_Should_Return_UnauthorizedResult_If_GrantType_Is_password_And_TokenService_Throws_UserPasswordMissmatchException()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.CreateUserRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .Throws<UserPasswordMissmatchException>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            IActionResult result = await this.tokenController.Auth(this.CreateTokenRequestModelWithPasswordGrantType());

            this.Annihilate();

            var unauthorizedResult = result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Auth_Should_Return_UnauthorizedResult_If_GrantType_Is_refresh_token_And_TokenService_Throws_UserTokenNotFoundException()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.ReplaceUserRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                                 .Throws<UserTokenNotFoundException>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            var model = this.CreateTokenRequestModelWithPasswordGrantType();
            model.grant_type = "refresh_token";

            IActionResult result = await this.tokenController.Auth(model);

            this.Annihilate();

            var unauthorizedResult = result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Auth_Should_Return_UnauthorizedResult_If_GrantType_Is_refresh_token_And_TokenService_Throws_UserNotFoundException()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.ReplaceUserRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                                 .Throws<UserNotFoundException>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            var model = this.CreateTokenRequestModelWithPasswordGrantType();
            model.grant_type = "refresh_token";

            IActionResult result = await this.tokenController.Auth(model);

            this.Annihilate();

            var unauthorizedResult = result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Auth_Should_Add_The_User_To_The_CurrentlyLoggedInUsersSingleton_If_Grant_Type_Is_password()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.CreateUserRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(Task.FromResult(new Token() { UserId = "user-id" }));

            this.Arrange_Configuration();

            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configuration);

            IActionResult result = await this.tokenController.Auth(this.CreateTokenRequestModelWithPasswordGrantType());

            int count = CurrentlyLoggedInUsersSingleton.GetAllUsernames().Count();

            this.Annihilate();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task Auth_Should_Not_Add_The_User_To_The_CurrentlyLoggedInUsersSingleton_If_Grant_Type_Is_refresh_token()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.ReplaceUserRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(Task.FromResult(new Token() { UserId = "user-id", Value = "token-value" }));

            this.Arrange_Configuration();

            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configuration);

            var model = this.CreateTokenRequestModelWithPasswordGrantType();
            model.grant_type = "refresh_token";

            IActionResult result = await this.tokenController.Auth(model);

            int keysCount = CurrentlyLoggedInUsersSingleton.GetAllUsernames().Count();

            this.Annihilate();

            Assert.Equal(0, keysCount);
        }

        [Fact]
        public async Task Auth_Should_Return_JsonResult_With_TokenResponseViewModel_If_Grant_Type_Is_password()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.CreateUserRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(Task.FromResult(new Token() { UserId = "user-id", Value = "token-value" }));

            this.Arrange_Configuration();

            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configuration);

            IActionResult actionResult = await this.tokenController.Auth(this.CreateTokenRequestModelWithPasswordGrantType());

            this.Annihilate();

            var jsonResult = actionResult.Should().BeOfType<JsonResult>();

            JsonResult result = actionResult as JsonResult;

            TokenResponseViewModel tokenResponse = result.Value as TokenResponseViewModel;

            Assert.NotNull(tokenResponse);
        }

        [Fact]
        public async Task Auth_Should_Return_JsonResult_With_TokenResponseViewModel_If_Grant_Type_Is_refresh_token()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.tokenServiceMock.Setup(m => m.ReplaceUserRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(Task.FromResult(new Token() { UserId = "user-id", Value = "token-value" }));

            this.Arrange_Configuration();

            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configuration);

            var model = this.CreateTokenRequestModelWithPasswordGrantType();
            model.grant_type = "refresh_token";

            IActionResult actionResult = await this.tokenController.Auth(model);

            this.Annihilate();

            var jsonResult = actionResult.Should().BeOfType<JsonResult>();

            JsonResult result = actionResult as JsonResult;

            TokenResponseViewModel tokenResponse = result.Value as TokenResponseViewModel;

            Assert.NotNull(tokenResponse);
        }

        private void Arrange_Configuration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            configurationBuilder.AddJsonFile(newPath + "appSettings.json");

            this.configuration = configurationBuilder.Build();
        }

        private TokenRequestViewModel CreateTokenRequestModelWithPasswordGrantType()
        {
            var tokenRequest = new TokenRequestViewModel();

            tokenRequest.grant_type = "password";
            tokenRequest.password = "Password123*&";
            tokenRequest.refresh_token = "refresh_token_value";
            tokenRequest.client_id = "client-id";
            tokenRequest.client_secret = "client-secret";
            tokenRequest.username = "username";

            return tokenRequest;
        }

        private TokenRequestViewModel CreateNullTokenRequestModel()
        {
            return null;
        }

        private void Annihilate()
        {
            this.tokenServiceMock = null;
            this.configurationMock = null;
            this.configuration = null;
            this.tokenServiceMock = null;

            var allUsernames = CurrentlyLoggedInUsersSingleton.GetAllUsernames();

            foreach (var username in allUsernames)
            {
                CurrentlyLoggedInUsersSingleton.RemoveEntryByKey(username);
            }
        }
    }
}