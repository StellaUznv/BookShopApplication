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
                    ZipCode = model.ZipCode,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };

                isCreated = await _locationRepository.AddAsync(location);
            }
            
            return isCreated;
        }

        public async Task<EditLocationViewModel> GetLocationToEditAsync(Guid id)
        {
            var location = await _locationRepository.FirstOrDefaultAsync(l => l.Id == id);

            var model = new EditLocationViewModel
            {
                Id = id,
                Address = location.Address,
                CityName = location.CityName,
                CountryName = location.CountryName,
                ZipCode = location.ZipCode,
                Latitude = location.Latitude,
                Longitude = location.Longitude

            };
            return model;
        }

        public async Task<bool> EditLocationAsync(EditLocationViewModel model)
        {
            var location = await _locationRepository.FirstOrDefaultAsync(l => l.Id == model.Id);

            location.Latitude = model.Latitude;
            location.Longitude = model.Longitude;
            location.CityName = model.CityName;
            location.CountryName = model.CountryName;
            location.ZipCode = model.ZipCode;
            location.Address = model.Address;

            return await _locationRepository.UpdateAsync(location);
        }

        public async Task<IEnumerable<LocationViewModel>> GetAllLocationsAsync()
        {
            var locations = await _locationRepository.GetAllAsync();

            var models = locations.Select(l => new LocationViewModel
            {
                CityName = l.CityName,
                CountryName = l.CountryName,
                ZipCode = l.ZipCode,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                Address = l.Address,
                Id = l.Id
            }).ToList();

            return models;
        }
    }
}
