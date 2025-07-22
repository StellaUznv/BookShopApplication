using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Web.ViewModels.Shop;

namespace BookShopApplication.Services.Contracts
{
    public interface IShopService
    {
        public Task<IEnumerable<ShopViewModel>> DisplayAllShopsAsync();

        public Task<bool> CreateShopAsync(CreateShopViewModel model, Guid userId, Guid locationId);

        public Task<IEnumerable<ShopViewModel>> GetManagedShopsAsync(Guid userId);

        public Task<ShopWithBooksViewModel> DisplayShopAsync(Guid shopId, Guid? userId);

        public Task<EditShopViewModel> GetShopToEditAsync(Guid shopId);

        public Task<bool> EditShopAsync(EditShopViewModel model);

        public Task<ShopBooksViewModel> GetBooksByShopIdAsync(Guid shopId);

        public Task<bool> DeleteShopAsync(Guid shopId);

    }
}
