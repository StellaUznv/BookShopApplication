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

namespace BookShopApplication.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync()
        {
            var books = await _context.Books.Select(b => new BookViewModel
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

        public async Task<BookDetailsViewModel> DisplayBookDetailsByIdAsync(Guid id)
        {
            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.BookInShops)
                .ThenInclude(bs => bs.Shop)
                .SingleOrDefaultAsync(b => b.Id == id);

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


    }
}
