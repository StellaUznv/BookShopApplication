using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Location;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookShopApplication.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookInShopRepository _bookInShopRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILocationRepository _locationRepository;

        public ShopService(IShopRepository shopRepository, IWishlistRepository wishlistRepository, ICartRepository cartRepository, 
            IBookInShopRepository bookInShopRepository, IBookRepository bookRepository, ILocationRepository locationRepository)
        {
            _shopRepository = shopRepository;
            _wishlistRepository = wishlistRepository;
            _cartRepository = cartRepository;
            _bookInShopRepository = bookInShopRepository;
            _bookRepository = bookRepository;
            _locationRepository = locationRepository;
        }


        public async Task<PaginatedList<ShopViewModel>> DisplayAllShopsAsync(int page, int pageSize)
        {
            var shopsModels = _shopRepository.GetAllAttached().Select(s => new ShopViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Name = s.Name,
                Latitude = s.Location.Latitude,
                Longitude = s.Location.Longitude,
                LocationAddress = s.Location.Address,
                LocationCity = s.Location.CityName
            });

            return await PaginatedList<ShopViewModel>.CreateAsync(shopsModels, page, pageSize);
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

        public async Task<bool> CreateShopAsAdminAsync(CreateShopAsAdminViewModel model)
        {
            var location = new Location
            {
                Id = model.Location.Id,
                Address = model.Location.Address,
                CityName = model.Location.CityName,
                CountryName = model.Location.CountryName,
                Latitude = model.Location.Latitude,
                Longitude = model.Location.Longitude,
                ZipCode = model.Location.ZipCode
            };
            bool locationCreated = await _locationRepository.AddAsync(location);

            if (locationCreated)
            {
                var shop = new Shop
                {
                    Name = model.Name,
                    Description = model.Description,
                    LocationId = location.Id,
                    ManagerId = model.SelectedManagerId
                };
                
                var shopCreated = await _shopRepository.AddAsync(shop);
                return shopCreated;
            }

            return false;
        }

        public async Task<IEnumerable<ShopWithBooksViewModel>> GetAllShopsWithBooksAsync()
        {
            var shops = await _shopRepository.GetAllAttached()
                .Include(s => s.Location)
                .Include(s => s.BooksInShop)
                .ThenInclude(bs => bs.Book)
                .ThenInclude(b => b.Genre)
                .ToListAsync();

            var models = shops.Select(s => new ShopWithBooksViewModel
            {
                BooksInShop = s.BooksInShop.Select(bi => new BookViewModel
                {
                    Id = bi.BookId,
                    Author = bi.Book.AuthorName,
                    Genre = bi.Book.Genre.Name,
                    ImagePath = bi.Book.ImagePath,
                    Price = bi.Book.Price.ToString("f2"),
                    Title = bi.Book.Title

                }).ToList(),
                Description = s.Description,
                Name = s.Name,
                Id = s.Id,
                Latitude = s.Location.Latitude,
                Longitude = s.Location.Longitude,
                LocationAddress = s.Location.Address,
                LocationCity = s.Location.CityName
            }).ToList();

            return models;
        }

        public async Task<ShopWithBooksViewModel> DisplayShopAsync(Guid shopId, Guid? userId)
        {
            var shop = await _shopRepository.GetAllAttached()
                .Include(s => s.Location)
                .Include(s => s.BooksInShop)
                .ThenInclude(bs => bs.Book)
                .ThenInclude(b => b.Genre)
                .FirstAsync(s => s.Id == shopId);

            if (userId != null)
            {
                Guid id = userId.Value;
                var wishlistItems = await _wishlistRepository.GetWishListedItemsIdsAsNoTrackingAsync(id);
                var cartItems = await _cartRepository.GetCartItemsIdsAsNoTrackingAsync(id);

                return new ShopWithBooksViewModel
                {
                    Id = shop.Id,
                    Description = shop.Description,
                    Name = shop.Name,
                    Latitude = shop.Location.Latitude,
                    Longitude = shop.Location.Longitude,
                    LocationAddress = shop.Location.Address,
                    LocationCity = shop.Location.CityName,
                    BooksInShop = shop.BooksInShop.Select(bs => new BookViewModel
                    {
                        Id = bs.BookId,
                        Author = bs.Book.AuthorName,
                        Genre = bs.Book.Genre?.Name ?? "Unknown",
                        ImagePath = bs.Book.ImagePath,
                        IsInCart = cartItems.Contains(bs.Book.Id),
                        IsInWishlist = wishlistItems.Contains(bs.Book.Id),
                        Price = bs.Book.Price.ToString("f2"),
                        Title = bs.Book.Title
                    }).ToList()
                };
            }

            // Anonymous or unauthenticated user
            return new ShopWithBooksViewModel
            {
                Id = shop.Id,
                Description = shop.Description,
                Name = shop.Name,
                Latitude = shop.Location.Latitude,
                Longitude = shop.Location.Longitude,
                LocationAddress = shop.Location.Address,
                LocationCity = shop.Location.CityName,
                BooksInShop = shop.BooksInShop.Select(bs => new BookViewModel
                {
                    Id = bs.BookId,
                    Author = bs.Book.AuthorName,
                    Genre = bs.Book.Genre?.Name ?? "Unknown",
                    ImagePath = bs.Book.ImagePath,
                    Price = bs.Book.Price.ToString("f2"),
                    Title = bs.Book.Title
                }).ToList()
            };
        }


        public async Task<EditShopViewModel> GetShopToEditAsync(Guid id)
        {
            var shop = await _shopRepository.GetByIdAsync(id);

            var model = new EditShopViewModel
            {
                Description = shop.Description,
                Name = shop.Name,
                Id = shop.Id,
                LocationId = shop.LocationId
            };

            return model;
        }

        public async Task<EditShopAsAdminViewModel> GetShopToEditAsAdminAsync(Guid id)
        {
            var shop = await _shopRepository.GetByIdAsync(id);
            var location = await _locationRepository.GetByIdAsync(shop.LocationId);

            var model = new EditShopAsAdminViewModel
            {
                Name = shop.Name,
                Id = shop.Id,
                Description = shop.Description,
                SelectedManagerId = shop.ManagerId,
                Location = new EditLocationViewModel
                {
                    Address = location.Address,
                    CityName = location.CityName,
                    CountryName = location.CountryName,
                    Id = location.Id,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    ZipCode = location.ZipCode
                }
            };
            return model;
        }

        public async Task<bool> EditShopAsync(EditShopViewModel model)
        {
            var shop = await _shopRepository.FirstOrDefaultAsync(s => s.Id == model.Id);

            shop.Description = model.Description;
            shop.Name = model.Name;

            return await _shopRepository.UpdateAsync(shop);
        }

        public async Task<bool> EditShopAsAdminAsync(EditShopAsAdminViewModel model)
        {
            var location = await _locationRepository.GetByIdAsync(model.Location.Id);
            location.Address = model.Location.Address;
            location.CityName = model.Location.CityName;
            location.Latitude = model.Location.Latitude;
            location.Longitude = model.Location.Longitude;
            location.ZipCode = model.Location.ZipCode;
            location.CountryName = model.Location.CountryName;

            bool locationUpdated = await _locationRepository.UpdateAsync(location);

            var shop = await _shopRepository.GetByIdAsync(model.Id);
            shop.Description = model.Description;
            shop.Name = model.Name;
            shop.ManagerId = model.SelectedManagerId;
            shop.LocationId = location.Id;

            bool shopUpdated = await _shopRepository.UpdateAsync(shop);

            return locationUpdated && shopUpdated;

        }

        public async Task<ShopBooksViewModel> GetBooksByShopIdAsync(Guid shopId,int page,int pageSize)
        {

            var bookQuery = _shopRepository
                .GetAllAttached()
                .Where(s => s.Id == shopId)
                .SelectMany(s => s.BooksInShop.Select(bs => new BookViewModel
                {
                    Author = bs.Book.AuthorName,
                    Genre = bs.Book.Genre.Name,
                    Id = bs.BookId,
                    ImagePath = bs.Book.ImagePath,
                    Price = bs.Book.Price.ToString("f2"),
                    Title = bs.Book.Title
                }));

            var books = await PaginatedList<BookViewModel>.CreateAsync(bookQuery, page, pageSize);

            var model = new ShopBooksViewModel
            {
                Books = books,
                ShopId = shopId
            };
            return model;

        }

        public async Task<PaginatedList<ShopViewModel>> GetManagedShopsAsync(Guid userId, int page, int pageSize)
        {
            var shops = _shopRepository.GetAllAttached()
                .Where(s => s.ManagerId == userId)
                .Select(s => new ShopViewModel()
                {
                    Id = s.Id,
                    Description = s.Description,
                    Latitude = s.Location.Latitude,
                    Longitude = s.Location.Longitude,
                    LocationAddress = s.Location.Address,
                    LocationCity = s.Location.CityName,
                    Name = s.Name,

                });
            return await PaginatedList<ShopViewModel>.CreateAsync(shops, page, pageSize);
        }

        public async Task<bool> DeleteShopAsync(Guid shopId)
        {
            
                var shop = await _shopRepository
                    .GetAllAttached()
                    .Include(s=>s.Location)
                    .Include(s => s.BooksInShop)
                    .ThenInclude(bs => bs.Book)
                    .FirstOrDefaultAsync(s => s.Id == shopId);

                if (shop == null) return false;

                foreach (var bs in shop.BooksInShop)
                {
                    await _bookRepository.SoftDeleteAsync(bs.Book);

                    await _bookInShopRepository.SoftDeleteAsync(bs); // Mark the book as deleted
                }

            
                return await _locationRepository.SoftDeleteAsync(shop.Location) && await _shopRepository.SoftDeleteAsync(shop);


        }
        
        public async Task<bool> HasUserAnyShopsAsync(Guid userId)
        {
            if (await _shopRepository.AnyAsync(s=>s.ManagerId == userId && !s.IsDeleted))
            {
                return true;
            }

            return false;
        }

    }
}
