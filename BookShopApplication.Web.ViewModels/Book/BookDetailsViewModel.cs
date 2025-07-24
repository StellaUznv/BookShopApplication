using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Shop;

namespace BookShopApplication.Web.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? ImagePath { get; set; }
        public string PagesNumber { get; set; } = null!;
        public ICollection<ShopNameViewModel> AvailableInShops { get; set; } = new List<ShopNameViewModel>();
        public bool IsInWishlist { get; set; }
        public bool IsInCart { get; set; }
    }
}
