using System;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using VideoChatWebApp.Data;
using VideoChatWebApp.Data.TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    public class TokenController : BaseApiController
    {
        public TokenController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration) : base(context, roleManager, userManager, configuration)
        {
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody]TokenRequestViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            if (model.grant_type == "password")
            {
                return await GetToken(model);
            }

            return new UnauthorizedResult();
        }

        private async Task<IActionResult> GetToken(TokenRequestViewModel model)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(model.username);

                if (user == null && model.username.Contains("@"))
                {
                    user = await UserManager.FindByEmailAsync(model.username);
                }

                if (user == null || !await UserManager.CheckPasswordAsync(user, model.password))
                {
                    return new UnauthorizedResult();
                }

                DateTime now = DateTime.UtcNow;

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
                };

                var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: Configuration["Auth:Jwt:Issuer"],
                    audience: Configuration["Auth:Jwt:Audience"],
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
                    signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

                var response = new TokenResponseViewModel()
                {
                    token = encodedToken,
                    expiration = tokenExpirationMins
                };

                return Json(response);
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }
    }
}