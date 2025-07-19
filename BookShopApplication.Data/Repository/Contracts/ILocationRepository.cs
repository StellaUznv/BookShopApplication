using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface ILocationRepository : IRepository<Location, Guid> , IAsyncRepository<Location, Guid>
    {
    }
}
