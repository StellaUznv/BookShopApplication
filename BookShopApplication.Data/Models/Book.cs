using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required] 
        public string Title { get; set; } = null!;

        [Required] 
        public string Description { get; set; } = null!;

        [Required] 
        public string Author { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        [Required]
        public Guid GenreId { get; set; } 
        [Required]
        public int PagesNumber { get; set; }
        [Required]
        public ICollection<BookShop> BooksShops { get; set; } = new List<BookShop>();

    }
}
