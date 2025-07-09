using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Data;
using BookShopApplication.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookShopApplication.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            this._context = context;
        }


        public async Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync(Guid userId)
        {
            var wishlistItems = await GetWishListedItemsAsNoTrackingAsync(userId, _context);

            var books = await _context.Books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Author = b.AuthorName,
                Genre = b.Genre.Name,
                ImagePath = b.ImagePath,
                Price = b.Price.ToString("f2"),
                Title = b.Title,
                IsInWishlist = wishlistItems.Contains(b.Id)

            }).AsNoTracking()
                .ToListAsync();
            return books;
        }

        public async Task<BookDetailsViewModel> DisplayBookDetailsByIdAsync(Guid userId,Guid bookId)
        {
            var book = await GetBookByIdAsNoTrackingAsync(bookId,_context);

            if (book == null)
            {
                // Optional: throw or return null/empty view model
                throw new InvalidOperationException("Book not found.");
            }

            var wishlistItems = await GetWishListedItemsAsNoTrackingAsync(userId, _context);

            var model = new BookDetailsViewModel
            {
                Id = book.Id,
                Author = book.AuthorName,
                Genre = book.Genre.Name,
                ImagePath = book.ImagePath,
                Price = book.Price.ToString("f2"),
                Title = book.Title,
                AvailableInShops = book.BookInShops
                    .Select(bs => bs.Shop.Name)
                    .ToList(),
                Description = book.Description,
                PagesNumber = book.PagesNumber.ToString(),
                IsInWishlist = wishlistItems.Contains(book.Id)
            };

            return model;
        }


        private static async Task<Book?> GetBookByIdAsNoTrackingAsync(Guid id, ApplicationDbContext context)
        {
            //Includes navigation properties data.
            var book = await context.Books
                .Include(b => b.Genre)
                .Include(b => b.BookInShops)
                .ThenInclude(bs => bs.Shop).AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == id);

            return book;
        }

        private static async Task<List<Guid>> GetWishListedItemsAsNoTrackingAsync(Guid userId, ApplicationDbContext context)
        {
            var wishlistItems = await context.WishlistItems
                .Where(w => w.UserId == userId)
                .AsNoTracking()
                .Select(w => w.BookId)
                .ToListAsync();
            return wishlistItems;
        }
    }
}
