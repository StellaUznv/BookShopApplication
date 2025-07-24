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
    public class BookInShop : ISoftDeletable
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]
        [ForeignKey(nameof(BookId))] 
        public Book Book { get; set; } = null!;
        [Required]
        public Guid ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        [Required] 
        public Shop Shop { get; set; } = null!;
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
