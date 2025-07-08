using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models
{
    public class Shop
    {
        [Key]
        public Guid Id { get; set; }

        [Required] 
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; } = null!;
        [Required]
        public Guid LocationId { get; set; }
        [Required]
        public ICollection<BookInShop> BooksInShop { get; set; } = new List<BookInShop>();
    }
}
