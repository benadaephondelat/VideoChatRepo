namespace ServiceLayer
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;
    using DAL.Interfaces;
    using ServiceLayer.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class TokenService : ITokenService
    {
        private IApplicationDbContext context;
        private IAuthManager authManager;

        public TokenService(IApplicationDbContext context, IAuthManager authManager)
        {
            this.context = context;
            this.authManager = authManager;
        }

        public async Task<Token> CreateUserRefreshToken(string username, string password, string cliendId)
        {
            var user = await this.authManager.FindByNameAsync(username);

            if (user == null && username.Contains("@"))
            {
                user = await this.authManager.FindByEmailAsync(username);
            }

            if (user == null || await this.authManager.CheckPasswordAsync(user, password) == false)
            {
                throw new Exception(); //TODO throw custom exception
            }

            var refreshToken = CreateRefreshToken(cliendId, user.Id);

            this.context.Tokens.Add(refreshToken);
            await this.context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<IEnumerable<Token>> GetAllTokens()
        {
            var result = await this.context.Tokens.ToListAsync();

            return result;
        }

        public async Task<Token> ReplaceUserRefreshToken(string clientId, string currentRefreshToken)
        {
            var refreshToken = this.context.Tokens.FirstOrDefault(t => t.ClientId == clientId && t.Value == currentRefreshToken);

            if (refreshToken == null)
            {
                throw new Exception(); //TODO throw custom exception
            }

            var user = await this.authManager.FindByIdAsync(refreshToken.UserId);

            if (user == null)
            {
                throw new Exception(); //TODO throw custom exception
            }

            Token newRefreshToken = CreateRefreshToken(refreshToken.ClientId, refreshToken.UserId);

            this.context.Tokens.Remove(refreshToken);
            this.context.Tokens.Add(newRefreshToken);

            await this.context.SaveChangesAsync();

            return newRefreshToken;
        }

        private Token CreateRefreshToken(string clientId, string userId)
        {
            return new Token()
            {
                ClientId = clientId,
                UserId = userId,
                Type = 0,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}