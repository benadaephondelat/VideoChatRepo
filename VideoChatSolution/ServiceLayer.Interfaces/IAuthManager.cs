namespace ServiceLayer.Interfaces
{
    using System.Threading.Tasks;

    using Models;

    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;

    public interface IAuthManager
    {
        /// <summary>
        /// Finds a user by id
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>ApplicationUser</returns>
        Task<ApplicationUser> FindByIdAsync(string userId);

        /// <summary>
        /// Find user by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>ApplicationUser</returns>
        Task<ApplicationUser> FindByEmailAsync(string email);

        /// <summary>
        /// Find user by username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>ApplicationUser</returns>
        Task<ApplicationUser> FindByNameAsync(string username);

        /// <summary>
        /// Find user by User.Identity
        /// </summary>
        /// <param name="claims">ClaimsPrincipal principal</param>
        /// <returns>ApplicationUser</returns>
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claims);

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <param name="password">password</param>
        /// <returns>IdentityResult - boolean</returns>
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Adds a user to a specific role
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <param name="role">role</param>
        /// <returns>IdentityResult - boolean</returns>
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Checks if password match the current user's password
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}