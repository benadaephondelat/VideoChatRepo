namespace VideoChatWebAppTests
{
    using System.Threading.Tasks;

    using ServiceLayer.Interfaces;
    using TestMakerFreeWebApp.Controllers;
    using TestMakerFreeWebApp.ViewModels;

    using Moq;
    using Xunit;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class TokenControllerTests
    {
        private Mock<ITokenService> tokenServiceMock;
        private Mock<IConfiguration> configurationMock;
        private TokenController tokenController;

        [Fact]
        public async Task Auth_Should_Return_BadRequest_If_TokenRequestViewModel_Is_Null()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.configurationMock = new Mock<IConfiguration>();
            this.tokenController = new TokenController(this.tokenServiceMock.Object, this.configurationMock.Object);

            IActionResult result = await this.tokenController.Auth(CreateNullTokenRequestModel());

            var badRequestResult = result.Should().BeOfType<BadRequestResult>();
        }

        private TokenRequestViewModel CreateNullTokenRequestModel()
        {
            return null;
        }

        private void Annihilate()
        {
            this.tokenServiceMock = null;
            this.configurationMock = null;
            this.tokenServiceMock = null;
        }
    }
}