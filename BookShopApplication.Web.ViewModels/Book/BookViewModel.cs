using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Book
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Price { get; set; } = null!;

        public string? ImagePath { get; set; }

        public bool IsInWishlist = false;
    }
}
