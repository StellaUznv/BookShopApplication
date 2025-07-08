using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; } = null!;
        //todo double-check the type!!!
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public int Quantity { get; set; } = 1;
    }
}
