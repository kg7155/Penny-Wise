using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            builder.Entity<CategoryElement>().HasKey(x => new { x.CategoryID, x.ElementID });
        }

        public DbSet<Movie> Movie { get; set; }

        public DbSet<UserAccount> Accounts { get; set; }

        public DbSet<Element> Elements { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
