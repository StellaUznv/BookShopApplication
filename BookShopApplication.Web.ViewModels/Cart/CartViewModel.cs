using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Cart
{
    public class CartViewModel
    {
        public ICollection<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public decimal Total { get; set; }
    }
}
