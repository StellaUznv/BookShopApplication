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
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookShopApplication.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICartRepository _cartRepository;

        public ShopService(IShopRepository shopRepository, IWishlistRepository wishlistRepository, ICartRepository cartRepository)
        {
            _shopRepository = shopRepository;
            _wishlistRepository = wishlistRepository;
            _cartRepository = cartRepository;
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

        public async Task<ShopWithBooksViewModel> DisplayShopAsync(Guid shopId, Guid userId)
        {
            
            var shop = await _shopRepository.GetAllAttached()
                .Include(s => s.Location)
                .Include(s => s.BooksInShop)
                .ThenInclude(bs => bs.Book.Genre)
                .FirstAsync(s => s.Id == shopId);


            var wishlistItems = await _wishlistRepository.GetWishListedItemsIdsAsNoTrackingAsync(userId);
            var cartItems = await _cartRepository.GetCartItemsIdsAsNoTrackingAsync(userId);

            var model = new ShopWithBooksViewModel
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
                    Genre = bs.Book.Genre.Name,
                    ImagePath = bs.Book.ImagePath,
                    IsInCart = cartItems.Contains(bs.Book.Id),
                    IsInWishlist = wishlistItems.Contains(bs.Book.Id),
                    Price = bs.Book.Price.ToString("f2"),
                    Title = bs.Book.Title
                }).ToList()
            };

            return model;
        }

        public async Task<EditShopViewModel> GetShopToEditAsync(Guid id)
        {
            var shop = await _shopRepository.FirstOrDefaultAsync(s => s.Id == id);

            var model = new EditShopViewModel
            {
                Description = shop.Description,
                Name = shop.Name,
                Id = shop.Id,
                LocationId = shop.LocationId
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

        public async Task<ShopBooksViewModel> GetBooksByShopIdAsync(Guid shopId)
        {
            var shop = await _shopRepository.FirstOrDefaultAsync(s => s.Id == shopId);

            var bookModels = shop.BooksInShop.Select(bs => new BookViewModel
            {
                Author = bs.Book.AuthorName,
                Genre = bs.Book.Genre.Name,
                Id = bs.BookId,
                ImagePath = bs.Book.ImagePath,
                Price = bs.Book.Price.ToString("f2"),
                Title = bs.Book.Title
            }).ToList();

            var model = new ShopBooksViewModel
            {
                Books = bookModels,
                ShopId = shopId
            };
            return model;
        }


        public async Task<IEnumerable<ShopViewModel>> GetManagedShopsAsync(Guid userId)
        {
            var shops = await _shopRepository.GetAllAttached()
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
                    
                }).ToListAsync();
            return shops;
        }

        public async Task<bool> DeleteShopAsync(Guid shopId)
        {
            var shop = await _shopRepository.FirstOrDefaultAsync(s => s.Id == shopId);
            //track deletion time or deleted by user, set those here

            return await _shopRepository.SoftDeleteAsync(shop);
        }

    }
}
