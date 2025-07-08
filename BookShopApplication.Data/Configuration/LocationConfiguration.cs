using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
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
        }
    }
}
