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
        public Task<CartViewModel> DisplayAllCartItemsAsync(Guid userId, int page, int pageSize);

        public Task<bool> AddToCartAsync(Guid userId, Guid bookId);

        public Task<bool> RemoveFromCartByIdAsync(Guid itemId);

        public Task<bool> RemoveFromCartAsync(Guid userId, Guid itemId);

        public  Task<bool> MoveToWishlistByIdAsync(Guid itemId);

        public Task<bool> PurchaseBooksAsync(ICollection<CartItemViewModel> books);
    }
}
