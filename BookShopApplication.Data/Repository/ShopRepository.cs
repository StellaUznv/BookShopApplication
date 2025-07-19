using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;

namespace BookShopApplication.Data.Repository
{
    public class ShopRepository : BaseRepository<Shop, Guid>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
