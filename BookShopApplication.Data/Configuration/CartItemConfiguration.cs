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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasQueryFilter(c => !c.IsPurchased);

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User)
                .WithMany(u => u.Cart)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(c => c.IsPurchased)
                .IsRequired();
        }
    }
}
