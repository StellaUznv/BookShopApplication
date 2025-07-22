using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface IGenreRepository : IRepository<Genre, Guid>,IAsyncRepository<Genre,Guid>
    {
    }
}
