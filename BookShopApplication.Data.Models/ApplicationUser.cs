﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Comment("User's first name")]
        public string? FirstName { get; set; }

        [Comment("User's last name")]
        public string? LastName { get; set; }

        [Comment("User's Wishlist")]
        public ICollection<WishlistItem> Wishlist { get; set; } = new List<WishlistItem>();

        [Comment("User's Cart")]
        public ICollection<CartItem> Cart { get; set; } = new List<CartItem>();

        [Comment("User's managed shops")] 
        public ICollection<Shop> ManagedShops { get; set; } = new List<Shop>();

        [Comment("A collection of purchased CartItems")]
        public ICollection<PurchaseItemUser> PurchasedItemsByUser { get; set; } = new List<PurchaseItemUser>();

    }
}
