﻿namespace VideoChatWebApp.Controllers
{
    using DAL;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models;
    using TestMakerFreeWebApp.Controllers;

    public class TestController : BaseApiController
    {
        private ApplicationDbContext context;

        public TestController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration) : base(context, roleManager, userManager, configuration)
        {
            this.context = context;
        }

        [HttpGet("Test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}