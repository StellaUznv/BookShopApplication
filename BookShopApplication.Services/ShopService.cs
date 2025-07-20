using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
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


        public async Task<IEnumerable<ShopViewModel>> DisplayAllShopsAsync()
        {
            var shopsModels = await _shopRepository.GetAllAttached().Select(s => new ShopViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Name = s.Name,
                Latitude = s.Location.Latitude,
                Longitude = s.Location.Longitude,
                LocationAddress = s.Location.Address,
                LocationCity = s.Location.CityName
            }).AsNoTracking()
                .ToListAsync();

            return shopsModels;
        }

        public async Task<bool> CreateShopAsync(CreateShopViewModel model, Guid userId, Guid locationId)
        {
            var shop = new Shop
            {
                Id = Guid.NewGuid(),
                Description = model.Description,
                Name = model.Name,
                LocationId = locationId,
                ManagerId = userId
            };

            return await _shopRepository.AddAsync(shop);
        }

        public async Task<IEnumerable<ShopViewModel>> GetManagedShopsAsync(Guid userId)
        {
            var shops = await _shopRepository.GetAllAttached()
                .Where(s => s.ManagerId == userId)
                .Select(s => new ShopViewModel
                {
                    Id = s.Id,
                    Description = s.Description,
                    Latitude = s.Location.Latitude,
                    Longitude = s.Location.Longitude,
                    LocationAddress = s.Location.Address,
                    LocationCity = s.Location.CityName,
                    Name = s.Name
                }).ToListAsync();
            return shops;
        }
    }
}
