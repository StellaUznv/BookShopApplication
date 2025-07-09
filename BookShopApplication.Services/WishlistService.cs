using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data;
using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Wishlist;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;
        public WishlistService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<WishlistItemViewModel>> DisplayWishlistItemsAsync(Guid userId)
        {
            var items = await _context.WishlistItems
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

            bool alreadyExists = await _context.WishlistItems
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

            await _context.WishlistItems.AddAsync(wishlistItem);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
