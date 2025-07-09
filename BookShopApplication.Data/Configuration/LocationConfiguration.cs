using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.LocationConstraints;

namespace BookShopApplication.Data.Configuration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasQueryFilter(l => !l.IsDeleted);

            builder.HasKey(l => l.Id);


            builder.Property(l => l.CountryName)
                .IsRequired()
                .HasMaxLength(CountryNameMaxLength);

            builder.Property(l=>l.CityName)
                .IsRequired()
                .HasMaxLength(CityNameMaxLength);

            builder.Property(l=>l.ZipCode)
                .IsRequired()
                .HasMaxLength(ZipCodeMaxLength);

            builder.HasMany(l => l.Shops)
                .WithOne(s => s.Location)
                .HasForeignKey(s => s.LocationId);


            //Seeding
            builder.HasData(
                new Location { Id = SeedGuids.Loc1, CountryName = "USA", CityName = "New York", ZipCode = "10001" },
                new Location { Id = SeedGuids.Loc2, CountryName = "UK", CityName = "London", ZipCode = "E1 6AN" },
                new Location { Id = SeedGuids.Loc3, CountryName = "Canada", CityName = "Toronto", ZipCode = "M5V" }
            );
        }
    }
}
