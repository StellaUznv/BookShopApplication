using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.BookConstraints;
namespace BookShopApplication.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasQueryFilter(b => !b.IsDeleted);


            builder.HasKey(b => b.Id);

            
            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLength);
            
            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);
            
            builder.Property(b => b.AuthorName)
                .IsRequired()
                .HasMaxLength(AuthorNameMaxLength);

            builder.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,4)");

            builder.Property(b => b.PagesNumber)
                .IsRequired();

            builder.Property(b => b.IsDeleted)
                .IsRequired();

            builder.Property(b => b.ImagePath)
                .IsRequired(false)
                .HasMaxLength(ImageMaxLength);

            builder.HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId);

            builder.HasMany(b => b.BookInShops)
                .WithOne(bs => bs.Book)
                .HasForeignKey(bs => bs.BookId);


            //Seeding
            builder.HasData(
                new Book { Id = SeedGuids.Book1, Title = "The Hobbit", Description = "A fantasy book", AuthorName = "Tolkien", Price = 15.99M, PagesNumber = 310, GenreId = SeedGuids.Fantasy, ImagePath = "/images/books/TheHobbitBookCover.jpg" },
                new Book { Id = SeedGuids.Book2, Title = "Dune", Description = "A sci-fi classic", AuthorName = "Frank Herbert", Price = 19.99M, PagesNumber = 412, GenreId = SeedGuids.SciFi, ImagePath = "/images/books/DuneBookCover.jpg" },
                new Book { Id = SeedGuids.Book3, Title = "Sherlock Holmes", Description = "Mystery detective", AuthorName = "Arthur Conan Doyle", Price = 12.99M, PagesNumber = 230, GenreId = SeedGuids.Mystery, ImagePath = "/images/books/SherlockHolmesBookCover.jpg" },
                new Book { Id = SeedGuids.Book4, Title = "Ender's Game", Description = "Sci-fi military novel", AuthorName = "Orson Scott Card", Price = 14.99M, PagesNumber = 324, GenreId = SeedGuids.SciFi, ImagePath = "/images/books/Ender'sGameBookCover.jpg" },
                new Book { Id = SeedGuids.Book5, Title = "The Name of the Wind", Description = "Fantasy epic", AuthorName = "Patrick Rothfuss", Price = 18.99M, PagesNumber = 662, GenreId = SeedGuids.Fantasy, ImagePath = "/images/books/TheNameOfTheWindBookCover.jpg" },
                new Book { Id = SeedGuids.Book6, Title = "The Hound of the Baskervilles", Description = "A thrilling mystery novel", AuthorName = "Arthur Conan Doyle", Price = 10.99M, PagesNumber = 256, GenreId = SeedGuids.Mystery, ImagePath = "/images/books/TheHoundOfTheBaskervillesBookCover.jpg" },
                new Book { Id = SeedGuids.Book7, Title = "Foundation", Description = "Sci-fi foundation of a galactic empire", AuthorName = "Isaac Asimov", Price = 16.99M, PagesNumber = 296, GenreId = SeedGuids.SciFi ,ImagePath = "/images/books/FoundationBookCover.jpg" }
            );
        }
    }
}
