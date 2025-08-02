using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Wishlist;

namespace BookShopApplication.Services.Contracts
{
    public interface IWishlistService
    {
        public Task<PaginatedList<WishlistItemViewModel>> DisplayWishlistItemsAsync(Guid userId , int page, int pageSize);

        public Task<bool> AddToWishlistAsync(Guid userId,Guid bookId);

        public Task<bool> RemoveFromWishlistByIdAsync(Guid itemId);

        public Task<bool> RemoveFromWishlistAsync(Guid userId, Guid bookId);
    }
}
