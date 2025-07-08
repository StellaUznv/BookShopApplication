using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; }

        [Comment("Name of the Genre entity")]
        public string Name { get; set; } = null!;

        [Comment("Description of the Genre entity")]
        public string Description { get; set; } = null!;
        
        [Comment("Reference collection to books.")]
        public ICollection<Book> Books { get; set; } = new List<Book>();

        [Comment("Tells if the Genre is Soft Deleted or not")]
        public bool IsDeleted { get; set; } = false;
    }
}
