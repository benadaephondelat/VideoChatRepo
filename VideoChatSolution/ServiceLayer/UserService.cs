namespace ServiceLayer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;
    using DAL.Interfaces;
    using ServiceLayer.Interfaces;
    using Common.CustomExceptions.UserExceptions;
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

            user = this.PopulateUserData(username, displayName, email, DateTime.Now);

            await authManager.CreateAsync(user, password);

            await authManager.AddToRoleAsync(user, "RegisteredUser");

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

        private ApplicationUser PopulateUserData(string username, string displayName, string email, DateTime now)
        {
            return new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username,
                Email = email,
                DisplayName = displayName,
                CreatedDate = now,
                LastModifiedDate = now,
                EmailConfirmed = true,
                LockoutEnabled = false
            };
        }

        //TODO This method is too FRAGILE - refactor it
        private void ValidateUserPassword(string password)
        {
            if (password.Any(c => !char.IsLetterOrDigit(c)) != UserValidationConstants.DoesUserPasswordRequiresSymbol)
            {
                throw new UserPasswordValidationException(UserValidationConstants.DoesUserPasswordRequiresSymbolMessage);
            }

            if (password.Any(c => char.IsDigit(c)) != UserValidationConstants.DoesUserPasswordRequiresDigit)
            {
                throw new UserPasswordValidationException(UserValidationConstants.UserPasswordRequiresDigitMessage);
            }

            if (password.Any(c => char.IsUpper(c)) != UserValidationConstants.DoesUserPasswordRequiresUppercase)
            {
                throw new UserPasswordValidationException(UserValidationConstants.UserPasswordRequiresUpercaseMessage);
            }

            if (password.Any(c => char.IsLower(c)) != UserValidationConstants.DoesUserPasswordRequiresLowercase)
            {
                throw new UserPasswordValidationException(UserValidationConstants.UserPasswordRequiresLowercaseMessage);
            }

            if (password.Length < UserValidationConstants.UserPasswordLength)
            {
                throw new UserPasswordValidationException(UserValidationConstants.UserPasswordLengthMessage);
            }
        }
    }
}
