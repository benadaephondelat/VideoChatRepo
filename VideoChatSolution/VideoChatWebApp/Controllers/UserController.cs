using System;
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

namespace TestMakerFreeWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IAuthManager authManager;
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };

        public UserController(IUserService userService, IAuthManager authManager)
        {
            this.userService = userService;
            this.authManager = authManager;
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

            return Json(viewModel, jsonSettings);
        }

        [Authorize]
        [Route("api/User/GetUsers")]
        [HttpGet, Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var currentUser = await this.authManager.GetUserAsync(HttpContext.User);

            var allUsers = CurrentlyLoggedInUsersSingleton.GetAllUsernames()
                                                          .Where(v => string.Equals(v, currentUser.UserName, StringComparison.OrdinalIgnoreCase) == false)
                                                          .ToList();

            return Json(allUsers, jsonSettings);
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
