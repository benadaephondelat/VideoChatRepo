namespace VideoChatWebApp.Data
{
    using System;
    using System.Linq;
    using VideoChatWebApp.Data.TestMakerFreeWebApp.Data;

    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            if (dbContext.Users.Any() == false)
            {
                CreateUsers(dbContext);
            }
        }

        private static void CreateUsers(ApplicationDbContext dbContext)
        {
            var user_Admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@yahoo.com",
                CreatedDate = new DateTime(2016, 03, 01, 12, 30, 00),
                LastModifiedDate = DateTime.Now
            };

            dbContext.Users.Add(user_Admin);

            dbContext.SaveChanges();
        }
    }
}