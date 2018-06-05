namespace ServiceLayer.Interfaces
{
    using Models;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<ApplicationUser> RegisterNewUser(string username, string displayName, string email, string password);
    }
}
