using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Book;

namespace BookShopApplication.Services.Contracts
{
    public interface IBookService 
    {
        public Task<PaginatedList<BookViewModel>> DisplayAllBooksAsync(Guid? userId , int page, int pageSize);

        public Task<BookDetailsViewModel> DisplayBookDetailsByIdAsync(Guid? userId, Guid bookId);

        public Task<bool> CreateBookAsync(CreateBookViewModel model);

        public Task<EditBookViewModel> GetBookToEdit(Guid bookId, Guid shopId);

        public Task<bool> EditBookAsync(EditBookViewModel model);

        public Task<bool> DeleteBookAsync(Guid bookId);
        public Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync();

    }
}
