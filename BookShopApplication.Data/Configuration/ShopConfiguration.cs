using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
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
        }
    }
}
