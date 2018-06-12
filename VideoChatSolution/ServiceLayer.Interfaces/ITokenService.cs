namespace ServiceLayer.Interfaces
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Models;
    using Common.CustomExceptions.UserExceptions;

    public interface ITokenService
    {
        /// <summary>
        /// Creates a Token associated with a given user or throws an exception
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <param name="cliendId">The client id of the application</param>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="UserPasswordMissmatchException"></exception>
        /// <returns>Task<Token></Token></returns>
        Task<Token> CreateUserRefreshToken(string username, string password, string cliendId);

        Task<Token> ReplaceUserRefreshToken(string clientId, string currentRefreshToken);
    }
}