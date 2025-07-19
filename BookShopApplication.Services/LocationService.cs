using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Location;

namespace BookShopApplication.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            this._locationRepository = locationRepository;
        }

        public async Task<bool> CreateLocationAsync(CreateLocationViewModel model)
        {
            bool isCreated = false;

            if (model != null)
            {
                var location = new Location
                {
                    Id = model.Id,
                    Address = model.Address,
                    CityName = model.CityName,
                    CountryName = model.CountryName,
                    ZipCode = model.ZipCode
                };

                isCreated = await _locationRepository.AddAsync(location);
            }
            
            return isCreated;
        }
    }
}
