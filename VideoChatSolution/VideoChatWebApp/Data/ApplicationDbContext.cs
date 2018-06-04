//namespace VideoChatWebApp.Data
//{
//    using global::TestMakerFreeWebApp.Data;
//    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//    using Microsoft.EntityFrameworkCore;
//    using VideoChatWebApp.Data.TestMakerFreeWebApp.Data;

//    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//    {
//        public DbSet<Token> Tokens { get; set; }

//        public ApplicationDbContext(DbContextOptions options) : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
//            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Tokens).WithOne(i => i.User);

//            modelBuilder.Entity<Token>().ToTable("Tokens");
//            modelBuilder.Entity<Token>().Property(i => i.Id).ValueGeneratedOnAdd();
//            modelBuilder.Entity<Token>().HasOne(i => i.User).WithMany(u => u.Tokens);
//        }
//    }
//}