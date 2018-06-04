using System;
using System.Text;
using System.Linq;
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
using TestMakerFreeWebApp.Data;

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
                var user = await UserManager.FindByNameAsync(model.username);

                if (user == null && model.username.Contains("@"))
                {
                    user = await UserManager.FindByEmailAsync(model.username);
                }

                if (user == null || !await UserManager.CheckPasswordAsync(user, model.password))
                {
                    return new UnauthorizedResult();
                }

                var rt = CreateRefreshToken(model.client_id, user.Id);

                DbContext.Tokens.Add(rt);
                DbContext.SaveChanges();

                var t = CreateAccessToken(user.Id, rt.Value);
                return Json(t);
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
                var rt = DbContext.Tokens.FirstOrDefault(t => t.ClientId == model.client_id && t.Value == model.refresh_token);

                if (rt == null)
                {
                    return new UnauthorizedResult();
                }

                var user = await UserManager.FindByIdAsync(rt.UserId);

                if (user == null)
                {
                    return new UnauthorizedResult();
                }

                var rtNew = CreateRefreshToken(rt.ClientId, rt.UserId);

                DbContext.Tokens.Remove(rt);
                DbContext.Tokens.Add(rtNew);
                DbContext.SaveChanges();

                var response = CreateAccessToken(rtNew.UserId, rtNew.Value);

                return Json(response);
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }

        private Token CreateRefreshToken(string clientId, string userId)
        {
            return new Token()
            {
                ClientId = clientId,
                UserId = userId,
                Type = 0,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow
            };
        }

        private TokenResponseViewModel CreateAccessToken(string userId, string refreshToken)
        {
            DateTime now = DateTime.UtcNow;
 
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
            };

            var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"]));

            var token = new JwtSecurityToken
            (
                issuer: Configuration["Auth:Jwt:Issuer"],
                audience: Configuration["Auth:Jwt:Audience"],
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
                refresh_token = refreshToken
            };
        }
    }
}