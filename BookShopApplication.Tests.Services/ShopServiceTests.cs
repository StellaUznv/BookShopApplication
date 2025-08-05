using System.Linq.Expressions;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using BookShopApplication.Web.ViewModels.Location;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Mvc.Rendering;
using MockQueryable;
using Moq;
using MockQueryable.EntityFrameworkCore;
using MockQueryable.Moq;

namespace BookShopApplication.Tests.Services;

[TestFixture]
public class ShopServiceTests
{
    private Mock<IShopRepository> _shopRepoMock;
    private Mock<ILocationRepository> _locationRepoMock;
    private Mock<IBookRepository> _bookRepoMock;
    private Mock<IBookInShopRepository> _bookInShopRepoMock;
    private Mock<IWishlistRepository> _wishlistRepoMock;
    private Mock<ICartRepository> _cartRepoMock;

    private ShopService _service;

    [SetUp]
    public void Setup()
    {
        _shopRepoMock = new Mock<IShopRepository>();
        _locationRepoMock = new Mock<ILocationRepository>();
        _bookRepoMock = new Mock<IBookRepository>();
        _bookInShopRepoMock = new Mock<IBookInShopRepository>();
        _wishlistRepoMock = new Mock<IWishlistRepository>();
        _cartRepoMock = new Mock<ICartRepository>();

        _service = new ShopService(
            _shopRepoMock.Object,
            _wishlistRepoMock.Object,
            _cartRepoMock.Object,
            _bookInShopRepoMock.Object,
            _bookRepoMock.Object,
            _locationRepoMock.Object
        );
    }
    [Test]
    public async Task DisplayAllShopsAsync_ReturnsPaginatedShops()
    {
        // Arrange
        var location = new Location
        {
            Latitude = 42.0,
            Longitude = 23.0,
            Address = "123 Street",
            CityName = "Test City"
        };

        var shops = new List<Shop>
        {
            new Shop { Id = Guid.NewGuid(), Name = "Shop 1", Description = "Desc", Location = location },
            new Shop { Id = Guid.NewGuid(), Name = "Shop 2", Description = "Desc", Location = location },
            new Shop { Id = Guid.NewGuid(), Name = "Shop 3", Description = "Desc", Location = location }
        };

        var mockShopsQueryable = shops.BuildMock(); // Requires MockQueryable.Moq

        _shopRepoMock.Setup(r => r.GetAllAttached()).Returns(mockShopsQueryable);

        var page = 1;
        var pageSize = 2;

        // Act
        var result = await _service.DisplayAllShopsAsync(page, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count); // PageSize = 2
        Assert.AreEqual(2, result.TotalPages);
        Assert.AreEqual(1, result.PageIndex);
        Assert.IsTrue(result.HasNextPage);
        Assert.IsFalse(result.HasPreviousPage);
    }
    [Test]
    public async Task CreateShopAsync_ShouldReturnTrue_WhenSuccessful()
    {
        var model = new CreateShopViewModel { Name = "Shop", Description = "Test" };
        var userId = Guid.NewGuid();
        var locationId = Guid.NewGuid();

        _shopRepoMock.Setup(r => r.AddAsync(It.IsAny<Shop>())).ReturnsAsync(true);

        var result = await _service.CreateShopAsync(model, userId, locationId);

        Assert.IsTrue(result);
        _shopRepoMock.Verify(r => r.AddAsync(It.Is<Shop>(s => s.ManagerId == userId && s.LocationId == locationId)), Times.Once);
    }
    [Test]
    public async Task CreateShopAsAdminAsync_ShouldReturnTrue_WhenBothCreated()
    {
        var model = new CreateShopAsAdminViewModel
        {
            Name = "Shop",
            Description = "Desc",
            SelectedManagerId = Guid.NewGuid(),
            Location = new CreateLocationViewModel
            {
                Id = Guid.NewGuid(),
                Address = "Addr", CityName = "City", CountryName = "Country",
                Latitude = 1.1, Longitude = 2.2, ZipCode = "1234"
            }
        };

        _locationRepoMock.Setup(l => l.AddAsync(It.IsAny<Location>())).ReturnsAsync(true);
        _shopRepoMock.Setup(s => s.AddAsync(It.IsAny<Shop>())).ReturnsAsync(true);

        var result = await _service.CreateShopAsAdminAsync(model);

        Assert.IsTrue(result);
    }
    [Test]
    public async Task GetAllShopsWithBooksAsync_ReturnsPaginatedShopsWithBooks()
    {
        // Arrange
        var genre = new Genre { Name = "Fiction" };
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book Title",
            AuthorName = "Author",
            Genre = genre,
            ImagePath = "/img.jpg",
            Price = 10.99m
        };

        var location = new Location
        {
            Latitude = 42.0,
            Longitude = 23.0,
            Address = "123 Street",
            CityName = "Test City"
        };

        var bookInShop = new BookInShop { Book = book };
        var shop = new Shop
        {
            Id = Guid.NewGuid(),
            Name = "Shop 1",
            Description = "Desc",
            Location = location,
            BooksInShop = new List<BookInShop> { bookInShop }
        };

        var shopList = new List<Shop> { shop };

        var mockDbSet = shopList.BuildMockDbSet();
        // Simulate EF’s ToListAsync on IQueryable by returning list directly
        _shopRepoMock.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        var page = 1;
        var pageSize = 10;

        // Act
        var result = await _service.GetAllShopsWithBooksAsync(page, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        var shopVm = result.First();
        Assert.AreEqual("Shop 1", shopVm.Name);
        Assert.AreEqual("Book Title", shopVm.BooksInShop.First().Title);
    }
    [Test]
    public async Task DeleteShopAsync_ShouldDeleteRelatedEntities()
    {
        // Arrange
        var shopId = Guid.NewGuid();
        var book = new Book { Id = Guid.NewGuid() };
        var bookInShop = new BookInShop { Book = book };
        var location = new Location();

        var shop = new Shop
        {
            Id = shopId,
            BooksInShop = new List<BookInShop> { bookInShop },
            Location = location
        };

        var shopList = new List<Shop> { shop };

        var mockShopQueryable = shopList.BuildMockDbSet(); // <-- This is critical

        _shopRepoMock.Setup(r => r.GetAllAttached()).Returns(mockShopQueryable.Object);
        _bookRepoMock.Setup(r => r.SoftDeleteAsync(book)).ReturnsAsync(true);
        _bookInShopRepoMock.Setup(r => r.SoftDeleteAsync(bookInShop)).ReturnsAsync(true);
        _locationRepoMock.Setup(r => r.SoftDeleteAsync(location)).ReturnsAsync(true);
        _shopRepoMock.Setup(r => r.SoftDeleteAsync(shop)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteShopAsync(shopId);

        // Assert
        Assert.IsTrue(result);
    }
    [Test]
    public async Task DisplayShopAsync_ReturnsCorrectShopWithBooks_ForAuthenticatedUser() 
    {
    // Arrange
    var shopId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var genre = new Genre { Name = "Fiction" };
    var book = new Book { Id = Guid.NewGuid(), Title = "Book", AuthorName = "Author", Genre = genre, ImagePath = "/img.jpg", Price = 15 };
    var location = new Location { Address = "Addr", CityName = "City", Latitude = 1.1, Longitude = 2.2 };
    var shop = new Shop
    {
        Id = shopId,
        Name = "Shop",
        Description = "Desc",
        Location = location,
        BooksInShop = new List<BookInShop> { new BookInShop { Book = book, BookId = book.Id } }
    };

    var shopQuery = new List<Shop> { shop }.BuildMock(); // use MockQueryable.Moq
    _shopRepoMock.Setup(r => r.GetAllAttached()).Returns(shopQuery);

    _wishlistRepoMock.Setup(r => r.GetWishListedItemsIdsAsNoTrackingAsync(userId))
        .ReturnsAsync(new List<Guid> { book.Id });

    _cartRepoMock.Setup(r => r.GetCartItemsIdsAsNoTrackingAsync(userId))
        .ReturnsAsync(new List<Guid>());

    // Act
    var result = await _service.DisplayShopAsync(shopId, userId);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(shopId, result.Id);
    Assert.AreEqual("Shop", result.Name);
    Assert.AreEqual(1, result.BooksInShop.Count);
    Assert.IsTrue(result.BooksInShop.First().IsInWishlist);
    Assert.IsFalse(result.BooksInShop.First().IsInCart);
}
    [Test]
    public async Task DisplayShopAsync_ReturnsCorrectShopWithBooks_ForAnonymousUser()
{
    // Arrange
    var shopId = Guid.NewGuid();
    var genre = new Genre { Name = "Fiction" };
    var book = new Book { Id = Guid.NewGuid(), Title = "Book", AuthorName = "Author", Genre = genre, ImagePath = "/img.jpg", Price = 15 };
    var location = new Location { Address = "Addr", CityName = "City", Latitude = 1.1, Longitude = 2.2 };
    var shop = new Shop
    {
        Id = shopId,
        Name = "Shop",
        Description = "Desc",
        Location = location,
        BooksInShop = new List<BookInShop> { new BookInShop { Book = book, BookId = book.Id } }
    };

    var shopQuery = new List<Shop> { shop }.BuildMock();
    _shopRepoMock.Setup(r => r.GetAllAttached()).Returns(shopQuery);

    // Act
    var result = await _service.DisplayShopAsync(shopId, null);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(shopId, result.Id);
    Assert.AreEqual("Shop", result.Name);
    Assert.AreEqual("Book", result.BooksInShop.First().Title);
}
    [Test]
    public async Task GetShopToEditAsync_ReturnsMappedViewModel()
{
    var shopId = Guid.NewGuid();
    var shop = new Shop
    {
        Id = shopId,
        Name = "Test Shop",
        Description = "Desc",
        LocationId = Guid.NewGuid()
    };

    _shopRepoMock.Setup(r => r.GetByIdAsync(shopId)).ReturnsAsync(shop);

    var result = await _service.GetShopToEditAsync(shopId);

    Assert.IsNotNull(result);
    Assert.AreEqual(shopId, result.Id);
    Assert.AreEqual("Test Shop", result.Name);
}
    [Test]
    public async Task EditShopAsync_UpdatesShopAndReturnsTrue()
{
    var shopId = Guid.NewGuid();
    var shop = new Shop
    {
        Id = shopId,
        Name = "Old Name",
        Description = "Old Desc"
    };

    var model = new EditShopViewModel
    {
        Id = shopId,
        Name = "New Name",
        Description = "New Desc"
    };

    _shopRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
        .ReturnsAsync(shop);

    _shopRepoMock.Setup(r => r.UpdateAsync(shop)).ReturnsAsync(true);

    var result = await _service.EditShopAsync(model);

    Assert.IsTrue(result);
    Assert.AreEqual("New Name", shop.Name);
    Assert.AreEqual("New Desc", shop.Description);
}
    [Test]
    public async Task GetBooksByShopIdAsync_ReturnsPaginatedBooks()
{
    var shopId = Guid.NewGuid();
    var genre = new Genre { Name = "Sci-Fi" };
    var book = new Book { Id = Guid.NewGuid(), Title = "Space Book", AuthorName = "Author", Genre = genre, ImagePath = "/img.png", Price = 20.5m };
    var shop = new Shop
    {
        Id = shopId,
        BooksInShop = new List<BookInShop> { new BookInShop { Book = book, BookId = book.Id } }
    };

    var shopQueryable = new List<Shop> { shop }.BuildMock();
    _shopRepoMock.Setup(r => r.GetAllAttached()).Returns(shopQueryable);

    var result = await _service.GetBooksByShopIdAsync(shopId, 1, 10);

    Assert.IsNotNull(result);
    Assert.AreEqual(shopId, result.ShopId);
    Assert.AreEqual(1, result.Books.Count);
    Assert.AreEqual("Space Book", result.Books.First().Title);
}
    [Test]
    public async Task GetManagedShopsAsync_ReturnsPaginatedShops()
{
    var userId = Guid.NewGuid();
    var location = new Location { Latitude = 1.0, Longitude = 2.0, Address = "Addr", CityName = "City" };
    var shop = new Shop
    {
        Id = Guid.NewGuid(),
        Name = "Managed Shop",
        Description = "Test Desc",
        ManagerId = userId,
        Location = location
    };

    var shopQueryable = new List<Shop> { shop }.BuildMock();
    _shopRepoMock.Setup(r => r.GetAllAttached()).Returns(shopQueryable);

    var result = await _service.GetManagedShopsAsync(userId, 1, 10);

    Assert.IsNotNull(result);
    Assert.AreEqual(1, result.Count);
    Assert.AreEqual("Managed Shop", result.First().Name);
}
    [Test]
    public async Task EditShopAsAdminAsync_UpdatesShopAndLocationSuccessfully()
{
    // Arrange
    var shopId = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var managerId = Guid.NewGuid();

    var location = new Location
    {
        Id = locationId,
        Address = "Old Address",
        CityName = "Old City",
        CountryName = "Old Country",
        Latitude = 10,
        Longitude = 20,
        ZipCode = "0000"
    };

    var shop = new Shop
    {
        Id = shopId,
        Name = "Old Shop",
        Description = "Old Desc",
        ManagerId = Guid.NewGuid(),
        LocationId = locationId
    };

    var editModel = new EditShopAsAdminViewModel
    {
        Id = shopId,
        Name = "New Shop",
        Description = "New Description",
        SelectedManagerId = managerId,
        Location = new EditLocationViewModel
        {
            Id = locationId,
            Address = "New Address",
            CityName = "New City",
            CountryName = "New Country",
            Latitude = 55.5,
            Longitude = 66.6,
            ZipCode = "12345"
        },
        Managers = new List<SelectListItem>() // Not used in logic, but required by model
    };

    _locationRepoMock.Setup(r => r.GetByIdAsync(locationId)).ReturnsAsync(location);
    _shopRepoMock.Setup(r => r.GetByIdAsync(shopId)).ReturnsAsync(shop);
    _locationRepoMock.Setup(r => r.UpdateAsync(It.Is<Location>(l => l.Id == locationId))).ReturnsAsync(true);
    _shopRepoMock.Setup(r => r.UpdateAsync(It.Is<Shop>(s => s.Id == shopId))).ReturnsAsync(true);

    // Act
    var result = await _service.EditShopAsAdminAsync(editModel);

    // Assert
    Assert.IsTrue(result);

    // Optional: Verify property updates
    Assert.Multiple(() =>
    {
        Assert.AreEqual("New Address", location.Address);
        Assert.AreEqual("New City", location.CityName);
        Assert.AreEqual("New Country", location.CountryName);
        Assert.AreEqual("12345", location.ZipCode);
        Assert.AreEqual(55.5, location.Latitude);
        Assert.AreEqual(66.6, location.Longitude);

        Assert.AreEqual("New Shop", shop.Name);
        Assert.AreEqual("New Description", shop.Description);
        Assert.AreEqual(managerId, shop.ManagerId);
    });

    // Optional: Verify methods were called
    _locationRepoMock.Verify(r => r.UpdateAsync(location), Times.Once);
    _shopRepoMock.Verify(r => r.UpdateAsync(shop), Times.Once);
}
    [Test]
    public async Task GetShopToEditAsAdminAsync_ReturnsCompleteViewModel()
{
    // Arrange
    var shopId = Guid.NewGuid();
    var locationId = Guid.NewGuid();

    var location = new Location
    {
        Id = locationId,
        Address = "Addr",
        CityName = "City",
        CountryName = "Country",
        ZipCode = "0000",
        Latitude = 3.3,
        Longitude = 4.4
    };

    var shop = new Shop
    {
        Id = shopId,
        Name = "Shop Name",
        Description = "Desc",
        LocationId = locationId,
        Location = location
    };

    _shopRepoMock.Setup(r => r.GetByIdAsync(shopId)).ReturnsAsync(shop);

    _locationRepoMock.Setup(r => r.GetByIdAsync(locationId)).ReturnsAsync(location);

    // Act
    var result = await _service.GetShopToEditAsAdminAsync(shopId);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(shopId, result.Id);
    Assert.AreEqual("Shop Name", result.Name);
    Assert.IsNotNull(result.Location);
    Assert.AreEqual("City", result.Location.CityName);
    Assert.AreEqual("Country", result.Location.CountryName);
}

}