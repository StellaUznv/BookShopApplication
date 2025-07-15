using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface IBookRepository : IRepository<Book, Guid>, IAsyncRepository<Book, Guid>
    {
        //ToDo: Add corresponding methods to the service
    }
}
