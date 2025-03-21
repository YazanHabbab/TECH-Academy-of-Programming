using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TECH_Academy_of_Programming.Models;

namespace TECH_Academy_of_Programming.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public override DbSet<IdentityRole> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("CS"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            // Unmap unwanted tables to database
            var identityUserClaim = modelBuilder.Model.FindEntityType(typeof(IdentityUserClaim<string>));
            if (identityUserClaim != null)
                modelBuilder.Model.RemoveEntityType(identityUserClaim);

            var identityUserLogin = modelBuilder.Model.FindEntityType(typeof(IdentityUserLogin<string>));
            if (identityUserLogin != null)
                modelBuilder.Model.RemoveEntityType(identityUserLogin);

            var identityUserToken = modelBuilder.Model.FindEntityType(typeof(IdentityUserToken<string>));
            if (identityUserToken != null)
                modelBuilder.Model.RemoveEntityType(identityUserToken);

            var identityRoleClaim = modelBuilder.Model.FindEntityType(typeof(IdentityRoleClaim<string>));
            if (identityRoleClaim != null)
                modelBuilder.Model.RemoveEntityType(identityRoleClaim);

        }
    }
}
