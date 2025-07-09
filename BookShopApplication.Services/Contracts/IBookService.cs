using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Book;

namespace BookShopApplication.Services.Contracts
{
    public interface IBookService 
    {
        public Task<IEnumerable<BookViewModel>> DisplayAllBooksAsync();
    }
}
