using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Data;
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
                Id = b.Id.ToString(),
                Author = b.AuthorName,
                AvailableInShops = b.BookInShops
                    .Where(bs=>bs.BookId == b.Id)
                    .Select(bs=>bs.Shop.Name)
                    .ToList(),
                Description = b.Description,
                Genre = b.Genre.Name,
                ImagePath = b.ImagePath,
                Price = b.Price.ToString(),
                Title = b.Title

            }).AsNoTracking()
                .ToListAsync();

            return books;
        }

      
    }
}
