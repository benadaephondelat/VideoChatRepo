namespace ServiceLayer.Interfaces
{
    using System.Threading.Tasks;

    using Models;

    using Microsoft.AspNetCore.Identity;

    public interface IAuthManager
    {
        Task<ApplicationUser> FindByIdAsync(string userId);

        Task<ApplicationUser> FindByEmailAsync(string email);

        Task<ApplicationUser> FindByNameAsync(string username);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}