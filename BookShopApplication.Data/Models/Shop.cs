using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data.Models
{
    public class Shop
    {
        [Key]
        public Guid Id { get; set; }

        [Comment("Name of the Shop entity")]
        public string Name { get; set; } = null!;

        [Comment("Description of the Shop entity")]
        public string Description { get; set; } = null!;

        [Comment("Tells if the Shop is Soft Deleted or not")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Reference to the Location entity")]
        public Location Location { get; set; } = null!;
        [Comment("Foreign key of the Location entity")]
        public Guid LocationId { get; set; }
        [Comment("Reference collection to the BookInShop mapping table")]
        public ICollection<BookInShop> BooksInShop { get; set; } = new List<BookInShop>();
    }
}
