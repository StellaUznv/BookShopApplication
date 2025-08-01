﻿using BookShopApplication.Data;
using BookShopApplication.Data.Migrations;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<PaginatedList<BookViewModel>> DisplayAllBooksAsync(int page, int pageSize)
        {
            //Todo: Fix!!!
            var books = await GetAllBooksToDisplayAsync(_bookRepository);
            return await PaginatedList<BookViewModel>.CreateAsync(books, page, pageSize);
        }

        public async Task<PaginatedList<BookViewModel>> DisplayAllBooksAsync(Guid? userId,int page, int pageSize)
        {

            if (userId.HasValue)
            {
                return await PaginatedList<BookViewModel>.CreateAsync(await GetAllBooksForUserByUserId(userId.Value, _bookRepository, _wishlistRepository, _cartRepository),page,pageSize);
            }

            return await PaginatedList<BookViewModel>.CreateAsync( await GetAllBooksToDisplayAsync(_bookRepository), page, pageSize); 

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
            var bookId = Guid.NewGuid();

            // 1. Save Image File to wwwroot/images/books/
            string? savedImagePath = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName);
                var fileName = $"{bookId}{extension}";
                var relativePath = Path.Combine("images", "books", fileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                savedImagePath = "/" + relativePath.Replace("\\", "/");
            }

            bool bookAdded = false;
            var book = new Book
            {
                Id = bookId,
                Title = model.Title,
                Description = model.Description,
                AuthorName = model.Author,
                Price = model.Price,
                PagesNumber = model.PagesNumber,
                GenreId = model.GenreId!.Value, // Already validated
                ImagePath = savedImagePath ?? "", // Optional
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

        public async Task<EditBookViewModel> GetBookToEdit(Guid bookId, Guid shopId)
        {
            var book = await _bookRepository.FirstOrDefaultAsync(b => b.Id == bookId);
            var model = new EditBookViewModel
            {
                Id = book.Id,
                Author = book.AuthorName,
                Description = book.Description,
                GenreId = book.GenreId,
                ImagePath = book.ImagePath,
                PagesNumber = book.PagesNumber,
                Price = book.Price,
                ShopId = shopId,
                Title = book.Title
            };

            return model;
        }

        public async Task<bool> EditBookAsync(EditBookViewModel model)
        {
            // 1. Save Image File to wwwroot/images/books/
            string? savedImagePath = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName);
                var fileName = $"{model.Id}{extension}";
                var relativePath = Path.Combine("images", "books", fileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                savedImagePath = "/" + relativePath.Replace("\\", "/");
            }
            var book = await _bookRepository.FirstOrDefaultAsync(b => b.Id == model.Id);
            
            if (savedImagePath == null)
            {
                savedImagePath = book.ImagePath;
            }

            

            book.Title = model.Title;
            book.AuthorName = model.Author;
            book.Description = model.Description;
            book.GenreId = model.GenreId;
            book.ImagePath = savedImagePath;
            book.PagesNumber = model.PagesNumber;
            book.Price = model.Price;
            

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<bool> DeleteBookAsync(Guid bookId)
        {
            var book = await _bookRepository.GetAllAttached()
                .Include(b => b.BookInShops)
                .Where(b => b.Id == bookId)
                .FirstOrDefaultAsync();

            if (book == null) return false;

            foreach (var bs in book.BookInShops)
            {
                await _bookInShopRepository.SoftDeleteAsync(bs); // Mark the book as deleted
            }


            return await _bookRepository.SoftDeleteAsync(book);
        }


        //Private helping methods...

        private static async Task<IQueryable<BookViewModel>> GetAllBooksToDisplayAsync(IBookRepository bookRepository)
        {
            var books = bookRepository
                .GetAllAttached()
                .Include(b => b.Genre).Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Author = b.AuthorName,
                    Genre = b.Genre.Name,
                    ImagePath = b.ImagePath,
                    Price = b.Price.ToString("f2"),
                    Title = b.Title
                });


            return books;
        }

        private static async Task<IQueryable<BookViewModel>> GetAllBooksForUserByUserId(Guid userId,  IBookRepository repository 
            ,IWishlistRepository wishlistRepository , ICartRepository cartRepository)
        {

            //ToDo: Get these methods to their corresponding repositories!!!

            var wishlistItems = await wishlistRepository.GetWishListedItemsIdsAsNoTrackingAsync(userId);
            var cartItems = await cartRepository.GetCartItemsIdsAsNoTrackingAsync(userId);

            var userBooks = repository.GetAllAttached()
                .Include(g => g.Genre).Select(ub => new BookViewModel
                {
                    Id = ub.Id,
                    Author = ub.AuthorName,
                    Genre = ub.Genre.Name,
                    ImagePath = ub.ImagePath,
                    Price = ub.Price.ToString("f2"),
                    Title = ub.Title,
                    IsInCart = cartItems.Contains(ub.Id),
                    IsInWishlist = wishlistItems.Contains(ub.Id)
                });

            return userBooks;
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
                    .Select(bs => new ShopNameViewModel
                    {
                        Id = bs.ShopId,
                        Name = bs.Shop.Name
                    })
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
                    .Select(bs => new ShopNameViewModel
                    {
                        Id = bs.ShopId,
                        Name = bs.Shop.Name
                    })
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
                .ThenInclude(bs => bs.Shop)
                .AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == id);

            return book;
        }

    }
}
