using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Web.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Services.Contracts
{
    public interface IShopService
    {
        public Task<PaginatedList<ShopViewModel>> DisplayAllShopsAsync(int page, int pageSize);

        public Task<bool> CreateShopAsync(CreateShopViewModel model, Guid userId, Guid locationId);

        public Task<bool> CreateShopAsAdminAsync(CreateShopAsAdminViewModel model);

        public Task<PaginatedList<ShopViewModel>> GetManagedShopsAsync(Guid userId, int page, int pageSize);

        public Task<ShopWithBooksViewModel> DisplayShopAsync(Guid shopId, Guid? userId);

        public Task<EditShopViewModel> GetShopToEditAsync(Guid shopId);

        public Task<EditShopAsAdminViewModel> GetShopToEditAsAdminAsync(Guid id);

        public Task<bool> EditShopAsync(EditShopViewModel model);

        public Task<bool> EditShopAsAdminAsync(EditShopAsAdminViewModel model);

        public Task<ShopBooksViewModel> GetBooksByShopIdAsync(Guid shopId, int page,int pageSize);

        public Task<bool> DeleteShopAsync(Guid shopId);

        public Task<bool> HasUserAnyShopsAsync(Guid userId);
        public Task<IEnumerable<ShopWithBooksViewModel>> GetAllShopsWithBooksAsync();

    }
}
