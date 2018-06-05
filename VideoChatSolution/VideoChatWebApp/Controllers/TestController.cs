namespace VideoChatWebApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ServiceLayer.Interfaces;
    using System.Threading.Tasks;
    
    public class TestController : Controller
    {
        private ITokenService tokenService;

        public TestController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [Route("api/Test/Test")]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            var allTokens = await this.tokenService.GetAllTokens();

            return Ok(allTokens);
        }
    }
}