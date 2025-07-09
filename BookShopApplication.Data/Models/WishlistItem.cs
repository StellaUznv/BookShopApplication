using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data.Models
{
    public class WishlistItem
    {
        [Key]
        public Guid Id { get; set; }

        [Comment("Foreign key of Book")]
        public Guid BookId { get; set; }

        [Comment("Navigation property for Book")]
        public Book Book { get; set; } = null!;

        [Comment("Foreign key of ApplicationUser")]
        public Guid UserId { get; set; }

        [Comment("Navigation property for ApplicationUser")]
        public ApplicationUser User { get; set; } = null!;
    }
}
