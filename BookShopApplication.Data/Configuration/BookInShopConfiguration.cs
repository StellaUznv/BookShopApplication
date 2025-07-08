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
    public class BookInShopConfiguration : IEntityTypeConfiguration<BookInShop>
    {
        public void Configure(EntityTypeBuilder<BookInShop> builder)
        {
            builder.HasQueryFilter(bs => !bs.Book.IsDeleted);

            builder.HasKey(bs => new { bs.BookId, bs.ShopId });


            builder.HasOne(bs => bs.Book)
                .WithMany(b => b.BookInShops)
                .HasForeignKey(bs => bs.BookId);

            builder.HasOne(bs => bs.Shop)
                .WithMany(s => s.BooksInShop)
                .HasForeignKey(bs => bs.ShopId);
        }
    }
}
