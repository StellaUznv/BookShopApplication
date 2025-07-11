using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data;
using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<CartItemViewModel>> DisplayAllCartItemsAsync(Guid userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemViewModel
                {
                    BookId = c.BookId,
                    Id = c.Id,
                    ImagePath = c.Book.ImagePath,
                    Price = c.Book.Price.ToString("f2"),
                    Quantity = c.Quantity.ToString(),
                    Title = c.Book.Title,
                    UserId = c.UserId
                }).ToListAsync();
            return cartItems;
        }

        public async Task<bool> AddToCartAsync(Guid userId, Guid bookId)
        {
            bool alreadyExists = await _context.CartItems
                .AnyAsync(w => w.UserId == userId && w.BookId == bookId);

            if (alreadyExists)
            {
                return false;
            }

            bool isInWishlist = await _context.WishlistItems.AnyAsync(w => w.UserId == userId && w.BookId == bookId);
            
            if (isInWishlist)
            {
                var wishlistItem =
                    await _context.WishlistItems.FirstAsync(w => w.UserId == userId && w.BookId == bookId);

                _context.WishlistItems.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId,
                Quantity = 1,
                IsPurchased = false
            };

            await _context.CartItems.AddAsync(cartItem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFromCartByIdAsync(Guid itemId)
        {
            bool isRemoved = false;

            var itemToRemove = await GetCartItemAsync(itemId, _context);
            if (itemToRemove != null)
            {
                _context.CartItems.Remove(itemToRemove);

                return await _context.SaveChangesAsync() > 0;
            }

            return isRemoved;
        }

        public async Task<bool> RemoveFromCartAsync(Guid userId, Guid itemId)
        {
            bool isRemoved = false;

            var itemToRemove = await GetCartItemByIdsAsync(userId, itemId, _context);
            if (itemToRemove != null)
            {
                _context.CartItems.Remove(itemToRemove);

                return await _context.SaveChangesAsync() > 0;
            }

            return isRemoved;
        }

        public async Task<bool> MoveToWishlistByIdAsync(Guid itemId)
        {
            bool isRemoved = false;
            bool isAdded = false;

            var itemToRemove = await GetCartItemAsync(itemId, _context);

            var itemToAdd = new WishlistItem
            {
                Id = Guid.NewGuid(),
                BookId = itemToRemove.BookId,
                UserId = itemToRemove.UserId
            };

            if (itemToAdd != null)
            {
               await _context.WishlistItems.AddAsync(itemToAdd);

               isAdded = await _context.SaveChangesAsync() > 0;
            }

            if (itemToRemove != null)
            {
                _context.CartItems.Remove(itemToRemove);

                isRemoved = await _context.SaveChangesAsync() > 0;
            }

            return isAdded && isRemoved;

        }

        private static async Task<CartItem?> GetCartItemByIdsAsync(Guid userId, Guid itemId, ApplicationDbContext context)
        {
            var item = await context.CartItems
                .SingleOrDefaultAsync(w => w.UserId == userId && w.BookId == itemId);

            return item;
        }

        private static async Task<CartItem?> GetCartItemAsync(Guid itemId, ApplicationDbContext context)
        {
            var item = await context.CartItems
                .SingleOrDefaultAsync(w => w.Id == itemId);

            return item;

        }
    }
}
