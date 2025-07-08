using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models
{
    public class WishlistItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; } = null!;


        //todo: Check if the type should be string!!!
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
