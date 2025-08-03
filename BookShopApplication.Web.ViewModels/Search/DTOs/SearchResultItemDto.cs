using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Search.DTOs
{
    public class SearchResultItemDto
    {
        public string Type { get; set; } // Book, Shop, etc.
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; } // Optional
        public string Url { get; set; } // Where to link this result
    }
}
