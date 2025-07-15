using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;

namespace BookShopApplication.Data.Repository
{
    public class BookRepository : BaseRepository<Book, Guid> , IBookRepository
    {
        public BookRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
