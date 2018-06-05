namespace ServiceLayer
{
    using System;
    using System.Threading.Tasks;

    using Models;
    using DAL.Interfaces;
    using ServiceLayer.Interfaces;

    public class UserService : IUserService
    {
        private IApplicationDbContext context;
        private IAuthManager authManager;

        public UserService(IApplicationDbContext context, IAuthManager authManager)
        {
            this.context = context;
            this.authManager = authManager;
        }

        public async Task<ApplicationUser> RegisterNewUser(string username, string displayName, string email, string password)
        {
            //TODO Check input params

            ApplicationUser user = await authManager.FindByNameAsync(username);

            if (user != null)
            {
                throw new Exception("Username already exist"); //TODO Throw custom exceptions
            }

            user = await authManager.FindByEmailAsync(email);

            if (user != null)
            {
                throw new Exception("Email already exists."); //TODO Throw custom exceptions
            }

            var now = DateTime.Now;

            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username,
                Email = email,
                DisplayName = displayName,
                CreatedDate = now,
                LastModifiedDate = now
            };

            await authManager.CreateAsync(user, password);

            await authManager.AddToRoleAsync(user, "RegisteredUser");

            user.EmailConfirmed = true;
            user.LockoutEnabled = false;

            await context.SaveChangesAsync();

            return user;
        }
    }
}
