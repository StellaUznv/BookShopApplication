using BookShopApplication.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Shop> Shops { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<BookInShop> BookInShops { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global query filters for soft deletes
            builder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);
            builder.Entity<Genre>().HasQueryFilter(g => !g.IsDeleted);
            builder.Entity<Shop>().HasQueryFilter(s => !s.IsDeleted);
            builder.Entity<Location>().HasQueryFilter(l => !l.IsDeleted);
        }
    }
}
