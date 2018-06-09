namespace ServiceLayer
{
    using System;
    using System.Threading.Tasks;

    using Models;
    using DAL.Interfaces;
    using ServiceLayer.Interfaces;
    using Common.CustomExceptions.UserExceptions;
    using System.Linq;
    using Common.ValidationConstants;

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
            this.ValidateUserPassword(password);

            ApplicationUser user = await authManager.FindByNameAsync(username);

            user = await ValidateIfUserAlreadyExists(email, user);

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

        private async Task<ApplicationUser> ValidateIfUserAlreadyExists(string email, ApplicationUser user)
        {
            if (user != null && string.IsNullOrWhiteSpace(user.UserName) == false)
            {
                throw new UserAlreadyExistsException();
            }

            user = await authManager.FindByEmailAsync(email);

            if (user != null && string.IsNullOrWhiteSpace(user.Email) == false)
            {
                throw new UserAlreadyExistsException();
            }

            return user;
        }

        private void ValidateUserPassword(string password)
        {
            if (password.Any(c => char.IsDigit(c)) != UserValidationConstants.DoesUserPasswordRequiresDigit)
            {
                throw new UserPasswordValidationException();
            }

            if (password.Any(c => char.IsUpper(c)) != UserValidationConstants.DoesUserPasswordRequiresUppercase)
            {
                throw new UserPasswordValidationException(UserValidationConstants.UserPasswordRequiresUpercaseMessage);
            }

            if (password.Any(c => char.IsLower(c)) != UserValidationConstants.DoesUserPasswordRequiresLowercase)
            {
                throw new UserPasswordValidationException();
            }

            if (password.Any(c => char.IsLetterOrDigit(c)) != UserValidationConstants.DoesUserPasswordRequiresNonAlphanumeric)
            {
                throw new UserPasswordValidationException();
            }

            if (password.Length < UserValidationConstants.UserPasswordLength)
            {
                throw new UserPasswordValidationException();
            }
        }
    }
}
