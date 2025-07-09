using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Wishlist;

namespace BookShopApplication.Services.Contracts
{
    public interface IWishlistService
    {
        public Task<IEnumerable<WishlistItemViewModel>> DisplayWishlistItemsAsync(Guid userId);

        public Task<bool> AddToWishlistAsync(Guid userId,Guid bookId);
    }
}
