using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Wishlist;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        public WishlistService(ApplicationDbContext context,IWishlistRepository _wishlistRepository)
        {
            this._wishlistRepository = _wishlistRepository;
        }

        public async Task<IEnumerable<WishlistItemViewModel>> DisplayWishlistItemsAsync(Guid userId)
        {
            var items = await _wishlistRepository.GetAllAttached()
                .Where(w => w.UserId == userId)
                .Select(w => new WishlistItemViewModel
                {
                    Id = w.Id,
                    BookId = w.BookId,
                    ImagePath = w.Book.ImagePath,
                    Price = w.Book.Price.ToString("f2"),
                    Title = w.Book.Title,
                    UserId = w.UserId
                }).ToListAsync();

            return items;
        }

        public async Task<bool> AddToWishlistAsync(Guid userId, Guid bookId)
        {

            bool alreadyExists = await _wishlistRepository
                .AnyAsync(w => w.UserId == userId && w.BookId == bookId);

            if (alreadyExists)
            {
                return false;
            }

            var wishlistItem = new WishlistItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId
            };

            return await _wishlistRepository.AddAsync(wishlistItem); // Calls SaveChangesAsync.
        }

        public async Task<bool> RemoveFromWishlistByIdAsync(Guid itemId)
        {
            bool isRemoved = false;

            var itemToRemove = await GetWishlistItemAsync(itemId, _wishlistRepository);
            if (itemToRemove != null)
            {
                isRemoved = await _wishlistRepository.DeleteAsync(itemToRemove);// Calls SaveChangesAsync
                return isRemoved;
            }

            return isRemoved;
        }

        public async Task<bool> RemoveFromWishlistAsync(Guid userId, Guid itemId)
        {
            bool isRemoved = false; 

            var itemToRemove = await GetWishlistItemByIdsAsync(userId, itemId, _wishlistRepository);
            if (itemToRemove != null)
            {
                isRemoved = await _wishlistRepository.DeleteAsync(itemToRemove);// Calls SaveChangesAsync
                return isRemoved;
            }

            return isRemoved;
        }

        //Private Helping methods...
        private static async Task<WishlistItem?> GetWishlistItemByIdsAsync(Guid userId, Guid itemId, IWishlistRepository wishlistRepository)
        {
            var item = await wishlistRepository
                .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == itemId);

            return item;
        }

        private static async Task<WishlistItem?> GetWishlistItemAsync(Guid itemId, IWishlistRepository wishlistRepository)
        {
            var item = await wishlistRepository
                .FirstOrDefaultAsync(w => w.Id == itemId);

            return item;

        }
    }
}
