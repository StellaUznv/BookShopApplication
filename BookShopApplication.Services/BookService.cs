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
using BookShopApplication.Data.Repository.Contracts;

namespace BookShopApplication.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookRepository _bookRepository;
        public BookService(ApplicationDbContext context, IBookRepository repository)
        {
            this._context = context;
            this._bookRepository = repository;
        }


        public async Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync(Guid? userId)
        {

            if (userId.HasValue)
            {
                return await GetAllBooksForUserByUserIdAsNoTracking(userId.Value, _bookRepository, _context);
            }

            return await GetAllBooksToDisplayAsyncAsNoTracking(_bookRepository);

        }

        public async Task<BookDetailsViewModel> DisplayBookDetailsByIdAsync(Guid? userId,Guid bookId)
        {

            if (userId.HasValue)
            {
                return await GetBookDetailsViewModelByUserIdAsyncAsNoTracking(userId.Value, bookId, _context, _bookRepository);
            }

            return await GetBookDetailsViewModelByBookIdAsyncAsNoTracking(bookId, _bookRepository);
        }



        //Private helping methods...

        private static async Task<IEnumerable<BookViewModel>> GetAllBooksToDisplayAsyncAsNoTracking(IBookRepository bookRepository)
        {
            var books = await bookRepository.GetAllAttached().AsNoTracking().ToListAsync();

            var booksModels = books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Author = b.AuthorName,
                Genre = b.Genre.Name,
                ImagePath = b.ImagePath,
                Price = b.Price.ToString("f2"),
                Title = b.Title
            }).ToList();

            return booksModels;
        }

        private static async Task<IEnumerable<BookViewModel>> GetAllBooksForUserByUserIdAsNoTracking(Guid userId,  IBookRepository repository
            , ApplicationDbContext context)
        {

            //ToDo: Get these methods to their corresponding repositories!!!

            var wishlistItems = await GetWishListedItemsIdsAsNoTrackingAsync(userId, context);
            var cartItems = await GetCartItemsIdsAsNoTrackingAsync(userId, context);

            var userBooks = await repository.GetAllAttached().Include(g=>g.Genre).AsNoTracking()
                .ToListAsync();

            var userBooksModels = userBooks.Select(ub=> new BookViewModel
            {
                Id = ub.Id,
                Author = ub.AuthorName,
                Genre = ub.Genre.Name,
                ImagePath = ub.ImagePath,
                Price = ub.Price.ToString("f2"),
                Title = ub.Title,
                IsInCart = cartItems.Contains(ub.Id),
                IsInWishlist = wishlistItems.Contains(ub.Id)
            }).ToList();
            return userBooksModels;
        }

        private static async Task<BookDetailsViewModel> GetBookDetailsViewModelByBookIdAsyncAsNoTracking(Guid bookId, IBookRepository repository)
        {
            var book = await GetBookByIdAsNoTrackingAsync(bookId, repository);

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

        private static async Task<BookDetailsViewModel> GetBookDetailsViewModelByUserIdAsyncAsNoTracking(Guid userId, Guid bookId,
            ApplicationDbContext context, IBookRepository repository)
        {
            var book = await GetBookByIdAsNoTrackingAsync(bookId, repository);

            if (book == null)
            {
                // Optional: throw or return null/empty view model
                throw new InvalidOperationException("Book not found.");
            }

            //ToDo: Get these methods to their corresponding repositories!!!

            var wishlistItems = await GetWishListedItemsIdsAsNoTrackingAsync(userId, context);
            var cartItems = await GetCartItemsIdsAsNoTrackingAsync(userId, context);

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

        private static async Task<Book?> GetBookByIdAsNoTrackingAsync(Guid id, IBookRepository repository)
        {
            //Includes navigation properties data.
            var book = await repository.GetAllAttached()
                .Include(b => b.Genre)
                .Include(b => b.BookInShops)
                .ThenInclude(bs => bs.Shop).AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == id);

            return book;
        }

        //ToDo: Get these methods to their corresponding repositories!!!
        private static async Task<List<Guid>> GetWishListedItemsIdsAsNoTrackingAsync(Guid userId, ApplicationDbContext context)
        {
            var wishlistItems = await context.WishlistItems
                .Where(w => w.UserId == userId)
                .AsNoTracking()
                .Select(w => w.BookId)
                .ToListAsync();
            return wishlistItems;
        }

        private static async Task<List<Guid>> GetCartItemsIdsAsNoTrackingAsync(Guid userId, ApplicationDbContext context)
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
