using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Seeding;
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


            //Seeding
            builder.HasData(
                // Shop 1 - NY (3 books)
                new BookInShop { BookId = SeedGuids.Book1, ShopId = SeedGuids.Shop1 },
                new BookInShop { BookId = SeedGuids.Book2, ShopId = SeedGuids.Shop1 },
                new BookInShop { BookId = SeedGuids.Book3, ShopId = SeedGuids.Shop1 },

                // Shop 2 - London (4 books)
                new BookInShop { BookId = SeedGuids.Book4, ShopId = SeedGuids.Shop2 },
                new BookInShop { BookId = SeedGuids.Book5, ShopId = SeedGuids.Shop2 },
                new BookInShop { BookId = SeedGuids.Book6, ShopId = SeedGuids.Shop2 },
                new BookInShop { BookId = SeedGuids.Book7, ShopId = SeedGuids.Shop2 },

                // Shop 3 - Toronto (3 books)
                new BookInShop { BookId = SeedGuids.Book1, ShopId = SeedGuids.Shop3 },
                new BookInShop { BookId = SeedGuids.Book4, ShopId = SeedGuids.Shop3 },
                new BookInShop { BookId = SeedGuids.Book6, ShopId = SeedGuids.Shop3 }
            );
        }
    }
}
