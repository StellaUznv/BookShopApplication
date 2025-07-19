using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Shop;

namespace BookShopApplication.Services.Contracts
{
    public interface IShopService
    {
        public Task<IEnumerable<ShopViewModel>> DisplayAllShopsAsync(Guid? userId);
    }
}
