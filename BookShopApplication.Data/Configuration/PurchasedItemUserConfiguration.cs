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
    public class PurchasedItemUserConfiguration : IEntityTypeConfiguration<PurchaseItemUser>
    {
        public void Configure(EntityTypeBuilder<PurchaseItemUser> builder)
        {
            builder.HasQueryFilter(pi=> !pi.CartItem.IsDeleted);

            builder.HasKey(pi => new { pi.UserId, pi.CartItemId});


            builder.HasOne(pi => pi.User)
                .WithMany(u => u.PurchasedItemsByUser)
                .HasForeignKey(pi => pi.UserId);

            builder.HasOne(pi=>pi.CartItem)
                .WithMany(ci=>ci.PurchasedItemByUsers)
                .HasForeignKey(pi=> pi.CartItemId);
        }
    }
}
