using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.ShopConstraints;
namespace BookShopApplication.Data.Configuration
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasQueryFilter(s => !s.IsDeleted);

            builder.HasKey(s => s.Id);


            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            builder.HasOne(s => s.Location)
                .WithMany(l => l.Shops)
                .HasForeignKey(s => s.LocationId);

            builder.HasMany(s => s.BooksInShop)
                .WithOne(bs => bs.Shop)
                .HasForeignKey(bs => bs.ShopId);

            builder.HasOne(s => s.Manager)
                .WithMany(u => u.ManagedShops) // This must be defined in ApplicationUser
                .HasForeignKey(s => s.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            //Seeding
            builder.HasData(
                new Shop { Id = SeedGuids.Shop1, Name = "NY Bookstore", Description = "Books in NY", LocationId = SeedGuids.Loc1,  ManagerId = SeedGuids.Manager},
                new Shop { Id = SeedGuids.Shop2, Name = "London Reads", Description = "Books in London", LocationId = SeedGuids.Loc2 , ManagerId = SeedGuids.Manager },
                new Shop { Id = SeedGuids.Shop3, Name = "Toronto Pages", Description = "Books in Toronto", LocationId = SeedGuids.Loc3, ManagerId = SeedGuids.Manager }
            );
        }
    }
}
