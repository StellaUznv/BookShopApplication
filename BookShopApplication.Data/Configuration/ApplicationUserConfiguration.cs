using BookShopApplication.Data.Common;
using BookShopApplication.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.UserConstraints;
namespace BookShopApplication.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(FirstNameMaxLength);

            builder.Property(u => u.LastName)
                .HasMaxLength(LastNameMaxLength);

            var hasher = new PasswordHasher<ApplicationUser>();
            var managerUser = new ApplicationUser
            {
                Id = SeedGuids.Manager,
                UserName = "manager@bookshop.com",
                NormalizedUserName = "MANAGER@BOOKSHOP.COM",
                Email = "manager@bookshop.com",
                NormalizedEmail = "MANAGER@BOOKSHOP.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                FirstName = "Manager",
                LastName = "User"
            };
            managerUser.PasswordHash = hasher.HashPassword(managerUser, "Manager@123");

            builder.HasData(managerUser);
        }

    }
}
