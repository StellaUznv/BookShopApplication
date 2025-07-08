using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookShopApplication.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public ICollection<WishlistItem> Wishlist { get; set; } = new List<WishlistItem>();
        public ICollection<CartItem> Cart { get; set; } = new List<CartItem>();
    }
}
