using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShopApplication.Data.Configuration
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasQueryFilter(w => !w.Book.IsDeleted);

            builder.HasKey(w => w.Id);

            builder.HasOne(w => w.Book)
                .WithMany()
                .HasForeignKey(w => w.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.User)
                .WithMany(u => u.Wishlist)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
