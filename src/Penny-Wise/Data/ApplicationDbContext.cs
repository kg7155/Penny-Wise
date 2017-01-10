using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Penny_Wise.Models;

namespace Penny_Wise.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Movie> Movie { get; set; }

        public DbSet<UserAccount> Accounts { get; set; }

        public DbSet<Goal> Goals { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
