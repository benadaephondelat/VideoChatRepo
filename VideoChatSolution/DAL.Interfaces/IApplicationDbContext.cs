namespace DAL.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using Models;

    using Microsoft.EntityFrameworkCore;

    public interface IApplicationDbContext
    {
        DbSet<Token> Tokens { get; set; }

        DbSet<ApplicationUser> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}