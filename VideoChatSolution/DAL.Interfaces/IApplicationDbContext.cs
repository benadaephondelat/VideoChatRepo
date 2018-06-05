namespace DAL.Interfaces
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using System.Threading;

    public interface IApplicationDbContext
    {
        DbSet<Token> Tokens { get; set; }

        DbSet<ApplicationUser> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}