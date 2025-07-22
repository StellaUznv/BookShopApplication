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
using BookShopApplication.Data.Migrations;
using BookShopApplication.Data.Repository.Contracts;

namespace BookShopApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookInShopRepository _bookInShopRepository;
        public BookService(IBookRepository repository, IWishlistRepository wishlistRepository, ICartRepository cartRepository, IBookInShopRepository bookInShopRepository)
        {
            this._bookRepository = repository;
            this._wishlistRepository = wishlistRepository;
            this._cartRepository = cartRepository;
            this._bookInShopRepository = bookInShopRepository;
        }


        public async Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync(Guid? userId)
        {

            if (userId.HasValue)
            {
                return await GetAllBooksForUserByUserIdAsNoTracking(userId.Value, _bookRepository, _wishlistRepository, _cartRepository);
            }

            return await GetAllBooksToDisplayAsyncAsNoTracking(_bookRepository);

        }

        public async Task<BookDetailsViewModel> DisplayBookDetailsByIdAsync(Guid? userId,Guid bookId)
        {

            if (userId.HasValue)
            {
                return await GetBookDetailsViewModelByUserIdAsyncAsNoTracking(userId.Value, bookId, _bookRepository,_wishlistRepository, _cartRepository);
            }

            return await GetBookDetailsViewModelByBookIdAsyncAsNoTracking(bookId, _bookRepository);
        }

        public async Task<bool> CreateBookAsync(CreateBookViewModel model)
        {
            bool bookAdded = false;
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                AuthorName = model.Author,
                Price = model.Price,
                PagesNumber = model.PagesNumber,
                GenreId = model.GenreId!.Value, // Already validated
                ImagePath = model.ImagePath ?? "", // Optional
                IsDeleted = false
            };
            bookAdded = await _bookRepository.AddAsync(book);

            bool bookInShopAdded = false;
            var bookShop = new BookInShop
            {
                BookId = book.Id,
                ShopId = model.ShopId
            };
            bookInShopAdded = await _bookInShopRepository.AddAsync(bookShop);
            return bookAdded && bookInShopAdded;
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
            ,IWishlistRepository wishlistRepository , ICartRepository cartRepository)
        {

            //ToDo: Get these methods to their corresponding repositories!!!

            var wishlistItems = await wishlistRepository.GetWishListedItemsIdsAsNoTrackingAsync(userId);
            var cartItems = await cartRepository.GetCartItemsIdsAsNoTrackingAsync(userId);

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
             IBookRepository repository, IWishlistRepository wishlistRepository, ICartRepository cartRepository)
        {
            var book = await GetBookByIdAsNoTrackingAsync(bookId, repository);

            if (book == null)
            {
                // Optional: throw or return null/empty view model
                throw new InvalidOperationException("Book not found.");
            }

            //ToDo: Get these methods to their corresponding repositories!!!

            var wishlistItems = await wishlistRepository.GetWishListedItemsIdsAsNoTrackingAsync(userId);
            var cartItems = await cartRepository.GetCartItemsIdsAsNoTrackingAsync(userId);

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

    }
}
