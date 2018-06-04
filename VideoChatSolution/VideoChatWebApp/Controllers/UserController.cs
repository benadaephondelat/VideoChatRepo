using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using TestMakerFreeWebApp.ViewModels;
using TestMakerFreeWebApp.Data;
using VideoChatWebApp.Data;
using VideoChatWebApp.Data.TestMakerFreeWebApp.Data;

namespace TestMakerFreeWebApp.Controllers
{
    public class UserController : BaseApiController
    {
        public UserController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration) : base(context, roleManager, userManager, configuration)
        {

        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]UserViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                return BadRequest("Username already exists");
            } 

            user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return BadRequest("Email already exists.");
            }

            var now = DateTime.Now;

            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.DisplayName,
                CreatedDate = now,
                LastModifiedDate = now
            };

            await UserManager.CreateAsync(user, model.Password);

            await UserManager.AddToRoleAsync(user, "RegisteredUser");

            user.EmailConfirmed = true;
            user.LockoutEnabled = false;

            DbContext.SaveChanges();

            UserViewModel viewModel = new UserViewModel();
            viewModel.DisplayName = user.DisplayName;
            viewModel.Email = user.Email;
            viewModel.UserName = user.Email;

            return Json(viewModel, JsonSettings);
        }
    }
}
