using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }


        public async Task<IEnumerable<ShopViewModel>> DisplayAllShopsAsync(Guid? userId)
        {
            var shopsModels = await _shopRepository.GetAllAttached().Select(s => new ShopViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Name = s.Name,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                LocationAddress = s.Location.Address,
                LocationCity = s.Location.CityName
            }).AsNoTracking()
                .ToListAsync();

            return shopsModels;
        }
    }
}
