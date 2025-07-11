using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Cart;

namespace BookShopApplication.Services.Contracts
{
    public interface ICartService
    {
        public Task<IEnumerable<CartItemViewModel>> DisplayAllCartItemsAsync(Guid userId);

        public Task<bool> AddToCartAsync(Guid userId, Guid bookId);
    }
}
