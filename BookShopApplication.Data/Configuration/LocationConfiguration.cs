using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Common;
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

            builder.Property(l=>l.Address)
                .IsRequired()
                .HasMaxLength(AddressMaxLength);

            builder.HasMany(l => l.Shops)
                .WithOne(s => s.Location)
                .HasForeignKey(s => s.LocationId);


            //Seeding
            builder.HasData(
                new Location { Id = SeedGuids.Loc1, CountryName = "USA", CityName = "New York", Address = "123 Broadway Ave, Manhattan" , ZipCode = "10001" , Latitude = 40.7128, Longitude = -74.0060},
                new Location { Id = SeedGuids.Loc2, CountryName = "UK", CityName = "London", Address = "456 Brick Lane, Shoreditch", ZipCode = "E1 6AN", Latitude = 51.5074, Longitude = -0.1278 },
                new Location { Id = SeedGuids.Loc3, CountryName = "Canada", CityName = "Toronto", Address = "789 King St W, Downtown", ZipCode = "M5V", Latitude = 43.6532, Longitude = -79.3832 }
            );
        }
    }
}
