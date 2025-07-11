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


        public async Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync(Guid? userId)
        {
            //todo: Simplify by adding private methods !!!

            if (userId.HasValue)
            {
                return await GetAllBooksForUserByUserIdAsNoTracking(userId.Value, _context);
            }

            return await GetAllBooksToDisplayAsyncAsNoTracking(_context);
        }

        
        public async Task<BookDetailsViewModel> DisplayBookDetailsByIdAsync(Guid? userId,Guid bookId)
        {

            //todo:Simplify by adding private methods!!!

            if (userId.HasValue)
            {
                return await GetBookDetailsViewModelByUserIdAsyncAsNoTracking(userId.Value, bookId, _context);
            }

            return await GetBookDetailsViewModelByBookIdAsyncAsNoTracking(bookId, _context);
        }



        //Private helping methods...

        private static async Task<IEnumerable<BookViewModel>> GetAllBooksToDisplayAsyncAsNoTracking(
            ApplicationDbContext context)
        {
            var books = await context.Books.Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Author = b.AuthorName,
                    Genre = b.Genre.Name,
                    ImagePath = b.ImagePath,
                    Price = b.Price.ToString("f2"),
                    Title = b.Title
                }).AsNoTracking()
                .ToListAsync();

            return books;
        }

        private static async Task<IEnumerable<BookViewModel>> GetAllBooksForUserByUserIdAsNoTracking(Guid userId, ApplicationDbContext context)
        {
            var wishlistItems = await GetWishListedItemsAsNoTrackingAsync(userId, context);
            var cartItems = await GetCartItemsAsNoTrackingAsync(userId, context);

            var userBooks = await context.Books.Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Author = b.AuthorName,
                    Genre = b.Genre.Name,
                    ImagePath = b.ImagePath,
                    Price = b.Price.ToString("f2"),
                    Title = b.Title,
                    IsInWishlist = wishlistItems.Contains(b.Id),
                    IsInCart = cartItems.Contains(b.Id)

                }).AsNoTracking()
                .ToListAsync();
            return userBooks;
        }

        private static async Task<BookDetailsViewModel> GetBookDetailsViewModelByBookIdAsyncAsNoTracking(Guid bookId,
            ApplicationDbContext context)
        {
            var book = await GetBookByIdAsNoTrackingAsync(bookId, context);

            if (book == null)
            {
                // Optional: throw or return null/empty view model
                throw new InvalidOperationException("Book not found.");
            }
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
                PagesNumber = book.PagesNumber.ToString()
            };
            return model;
        }

        private static async Task<BookDetailsViewModel> GetBookDetailsViewModelByUserIdAsyncAsNoTracking(Guid userId,Guid bookId,
            ApplicationDbContext context)
        {
            var book = await GetBookByIdAsNoTrackingAsync(bookId, context);

            if (book == null)
            {
                // Optional: throw or return null/empty view model
                throw new InvalidOperationException("Book not found.");
            }
            var wishlistItems = await GetWishListedItemsAsNoTrackingAsync(userId, context);
            var cartItems = await GetCartItemsAsNoTrackingAsync(userId, context);

            var userModel = new BookDetailsViewModel
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
                IsInWishlist = wishlistItems.Contains(book.Id),
                IsInCart = cartItems.Contains(book.Id)
            };
            return userModel;
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

        private static async Task<List<Guid>> GetCartItemsAsNoTrackingAsync(Guid userId, ApplicationDbContext context)
        {
            var cartItems = await context.CartItems
                .Where(c => c.UserId == userId)
                .AsNoTracking()
                .Select(c => c.BookId)
                .ToListAsync();
            return cartItems;
        }
    }
}
