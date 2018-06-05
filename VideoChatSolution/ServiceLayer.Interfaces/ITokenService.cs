namespace ServiceLayer.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface ITokenService
    {
        Task<Token> CreateUserRefreshToken(string username, string password, string cliendId);

        Task<Token> ReplaceUserRefreshToken(string clientId, string currentRefreshToken);

        Task<IEnumerable<Token>> GetAllTokens();
    }
}