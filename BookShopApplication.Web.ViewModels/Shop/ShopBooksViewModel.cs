using BookShopApplication.Web.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Shop
{
    public class ShopBooksViewModel
    {
        public Guid ShopId { get; set; }
        public PaginatedList<BookViewModel> Books { get; set; } = null!;
    }
}
