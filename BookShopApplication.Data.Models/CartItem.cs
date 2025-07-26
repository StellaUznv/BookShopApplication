using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data.Models
{
    public class CartItem : ISoftDeletable
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

        [Comment("Number of the same items to buy")]
        public int Quantity { get; set; } = 1;
        [Comment("A collection of users who bought the item")]
        public ICollection<PurchaseItemUser> PurchasedItemByUsers { get; set; } = new List<PurchaseItemUser>();
        [Comment("Tells if it's deleted or not")]
        public bool IsDeleted { get; set; }
    }
}
