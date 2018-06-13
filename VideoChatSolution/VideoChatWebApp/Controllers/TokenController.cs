using System;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Models;
using ServiceLayer.Interfaces;
using TestMakerFreeWebApp.ViewModels;
using VideoChatWebApp.Infrastrucure.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private ITokenService tokenService;
        private IConfiguration configuration;

        public TokenController(ITokenService tokenService, IConfiguration configuration)
        {
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody]TokenRequestViewModel model)
        {
            if (model == null) // TODO replace with modelstate check action filter
            {
                return BadRequest();
            }

            if (model.grant_type == "password")
            {
                 return await GetToken(model);
            }
            else if (model.grant_type == "refresh_token")
            {
                return await RefreshToken(model);
            }

            return new UnauthorizedResult();
        }

        private async Task<IActionResult> GetToken(TokenRequestViewModel model)
        {
            try
            {
                Token refreshToken = await this.tokenService.CreateUserRefreshToken(model.username, model.password, model.client_id);

                CurrentlyLoggedInUsersSingleton.AddNewEntry(model.username, refreshToken.UserId);

                TokenResponseViewModel tokenResponse = CreateAccessToken(refreshToken.UserId, refreshToken.Value, model.username);

                return Json(tokenResponse);
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }

        private async Task<IActionResult> RefreshToken(TokenRequestViewModel model)
        {
            try
            {
                Token newRefreshToken = await this.tokenService.ReplaceUserRefreshToken(model.client_id, model.refresh_token);

                var response = CreateAccessToken(newRefreshToken.UserId, newRefreshToken.Value, model.username);

                return Json(response);
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }

        private TokenResponseViewModel CreateAccessToken(string userId, string refreshToken, string username)
        {
            DateTime now = DateTime.UtcNow;
 
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
            };

            var tokenExpirationMins = this.configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Auth:Jwt:Key"]));

            var token = new JwtSecurityToken
            (
                issuer: this.configuration["Auth:Jwt:Issuer"],
                audience: this.configuration["Auth:Jwt:Audience"],
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
                signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponseViewModel()
            {
                token = encodedToken,
                expiration = tokenExpirationMins,
                refresh_token = refreshToken,
                username = username
            };
        }
    }
}