using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Cart
{
    public class CartViewModel
    {
        public PaginatedList<CartItemViewModel> Items { get; set; } = null!;

        public decimal Total { get; set; }
    }
}
