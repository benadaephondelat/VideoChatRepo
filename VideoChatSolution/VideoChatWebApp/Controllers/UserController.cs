using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

using Models;
using ServiceLayer.Interfaces;
using TestMakerFreeWebApp.ViewModels;
using VideoChatWebApp.Infrastrucure.Services;
using Common.CustomExceptions.UserExceptions;
using Common.CustomExceptions;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

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
            if (model == null) // TODO replace with modelstate check action filter
            {
                return BadRequest();
            }

            ApplicationUser user;

            try //TODO replace try-catch block with error filter
            {
                user = await this.userService.RegisterNewUser(model.UserName, model.DisplayName, model.Email, model.Password);
            }
            catch (VideoChatWebAppMainException e) when (e is UserAlreadyExistsException || e is UserPasswordValidationException)
            {
                return Conflict();
            }

            UserViewModel viewModel = this.CreateUserViewModel(user);

            return Json(viewModel, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [Route("api/Users/GetUsers")]
        [HttpGet, Authorize]
        public IActionResult GetUsers()
        {
            var allUsers = CurrentlyLoggedInUsersService.GetAllUsernames().ToList();

            return Json(allUsers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        private UserViewModel CreateUserViewModel(ApplicationUser user)
        {
            UserViewModel viewModel = new UserViewModel();
            viewModel.DisplayName = user.DisplayName;
            viewModel.Email = user.Email;
            viewModel.UserName = user.Email;
            return viewModel;
        }
    }
}
