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
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Price { get; set; } = null!;
        public ICollection<string> AvailableInShops { get; set; } = new List<string>();
    }
}
