using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using TestMakerFreeWebApp.ViewModels;
using DAL;
using Models;
using ServiceLayer.Interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using VideoChatWebApp.Infrastrucure.Services;
using System.Linq;

namespace TestMakerFreeWebApp.Controllers
{
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("api/User/Add")]
        [HttpPost()]
        public async Task<IActionResult> Add([FromBody]UserViewModel model)
        {
            //TODO CheckModelState

            var user = await this.userService.RegisterNewUser(model.UserName, model.DisplayName, model.Email, model.Password);

            UserViewModel viewModel = new UserViewModel();
            viewModel.DisplayName = user.DisplayName;
            viewModel.Email = user.Email;
            viewModel.UserName = user.Email;

            return Json(viewModel, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [Route("api/Users/GetUsers")]
        [HttpGet, Authorize]
        public IActionResult GetUsers()
        {
            //string username = HttpContext.User.Identity.Name;

            var allUsers = CurrentlyLoggedInUsersService.GetAllUsernames().ToList();

            return Json(allUsers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }
        
    }
}
