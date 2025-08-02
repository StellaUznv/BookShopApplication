using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Location;

namespace BookShopApplication.Services.Contracts
{
    public interface ILocationService
    {
        public Task<bool> CreateLocationAsync(CreateLocationViewModel model);

        public Task<EditLocationViewModel> GetLocationToEditAsync(Guid locationId);

        public Task<bool> EditLocationAsync(EditLocationViewModel model);
        public Task<PaginatedList<LocationViewModel>> GetAllLocationsAsync(int page, int pageSize);
    }
}
