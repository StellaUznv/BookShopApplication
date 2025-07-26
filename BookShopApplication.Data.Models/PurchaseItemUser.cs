using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models.Contracts;

namespace BookShopApplication.Data.Models
{
    public class PurchaseItemUser : ISoftDeletable
    {
        [Required]
        public Guid CartItemId { get; set; }
        [Required]
        [ForeignKey(nameof(CartItemId))]
        public CartItem CartItem { get; set; } = null!;
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
