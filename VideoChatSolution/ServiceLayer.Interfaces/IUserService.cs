namespace ServiceLayer.Interfaces
{
    using Models;
    using System.Threading.Tasks;
    using Common.CustomExceptions.UserExceptions;

    public interface IUserService
    {
        /// <summary>
        /// Registers a new user in the system - adds a new entry in the Users table.
        /// Or throws an exception.
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="email">Email of the users</param>
        /// <param name="password">Password of the user</param>
        /// <exception cref="UserAlreadyExistsException"></exception>
        /// <exception cref="UserPasswordValidationException"></exception>
        /// <returns>Task<ApplicationUser></ApplicationUser></returns>
        Task<ApplicationUser> RegisterNewUser(string username, string displayName, string email, string password);
    }
}
