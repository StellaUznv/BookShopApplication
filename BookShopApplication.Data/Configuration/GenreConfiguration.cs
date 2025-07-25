﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.GenreConstraints;

namespace BookShopApplication.Data.Configuration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasQueryFilter(g => !g.IsDeleted);

            builder.HasKey(g => g.Id);


            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder.Property(g => g.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);


            builder.HasMany(g => g.Books)
                .WithOne(b => b.Genre)
                .HasForeignKey(b => b.GenreId);


            //Seeding
            builder.HasData(
                new Genre { Id = SeedGuids.Fantasy, Name = "Fantasy", Description = "Fantasy genre" },
                new Genre { Id = SeedGuids.SciFi, Name = "Sci-Fi", Description = "Science Fiction" },
                new Genre { Id = SeedGuids.Mystery, Name = "Mystery", Description = "Mystery genre" }
            );
        }
    }
}
