using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookShopApplication.Data.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data.Models
{
    public class Book : ISoftDeletable
    {
        [Key]
        public Guid Id { get; set; }
        
        [Comment("Title of the Book entity")]
        public string Title { get; set; } = null!;
        
        [Comment("Description of the Book entity")]
        public string Description { get; set; } = null!;
        
        [Comment("The Author's name in the Book entity")]
        public string AuthorName { get; set; } = null!;
        
        [Comment("Price of the Book entity")]
        public decimal Price { get; set; }
        
        [Comment("Pages number in the Book entity")] 
        public int PagesNumber { get; set; }

        [Comment("The image's path")]
        public string? ImagePath { get; set; }

        [Comment("Tells if the Book is Soft Deleted or not")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Reference to the Genre entity")]
        public Genre Genre { get; set; } = null!;
        
        [Comment("Foreign key of Genre entity")]
        public Guid GenreId { get; set; } 
        
        [Comment("Reference collection to the BookInShop mapping table")]
        public ICollection<BookInShop> BookInShops { get; set; } = new List<BookInShop>();

    }
}
