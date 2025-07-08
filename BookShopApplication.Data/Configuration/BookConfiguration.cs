using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
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


            builder.HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId);

            builder.HasMany(b => b.BookInShops)
                .WithOne(bs => bs.Book)
                .HasForeignKey(bs => bs.BookId);
        }
    }
}
