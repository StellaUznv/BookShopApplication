using System.Linq.Expressions;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using BookShopApplication.Web.ViewModels.Location;
using Moq;

namespace BookShopApplication.Tests.Services;

[TestFixture]
public class LocationServiceTests
{
    private Mock<ILocationRepository> _locationRepositoryMock;
    private LocationService _locationService;

    [SetUp]
    public void SetUp()
    {
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _locationService = new LocationService(_locationRepositoryMock.Object);
    }

    [Test]
    public async Task CreateLocationAsync_WithValidModel_ShouldReturnTrue()
    {
        var model = new CreateLocationViewModel
        {
            Id = Guid.NewGuid(),
            Address = "Address1",
            CityName = "City1",
            CountryName = "Country1",
            ZipCode = "12345",
            Latitude = 10.0,
            Longitude = 20.0
        };

        _locationRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Location>()))
            .ReturnsAsync(true);

        var result = await _locationService.CreateLocationAsync(model);

        Assert.IsTrue(result);
        _locationRepositoryMock.Verify(r => r.AddAsync(It.Is<Location>(l =>
            l.Id == model.Id &&
            l.Address == model.Address &&
            l.CityName == model.CityName &&
            l.CountryName == model.CountryName &&
            l.ZipCode == model.ZipCode &&
            l.Latitude == model.Latitude &&
            l.Longitude == model.Longitude)), Times.Once);
    }

    [Test]
    public async Task CreateLocationAsync_WithNullModel_ShouldReturnFalse()
    {
        var result = await _locationService.CreateLocationAsync(null);

        Assert.IsFalse(result);
        _locationRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Location>()), Times.Never);
    }

    [Test]
    public async Task GetLocationToEditAsync_ShouldReturnEditLocationViewModel()
    {
        var locationId = Guid.NewGuid();
        var location = new Location
        {
            Id = locationId,
            Address = "Address1",
            CityName = "City1",
            CountryName = "Country1",
            ZipCode = "12345",
            Latitude = 10.0,
            Longitude = 20.0
        };

        _locationRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Location, bool>>>()))
            .ReturnsAsync(location);

        var result = await _locationService.GetLocationToEditAsync(locationId);

        Assert.IsNotNull(result);
        Assert.AreEqual(locationId, result.Id);
        Assert.AreEqual(location.Address, result.Address);
        Assert.AreEqual(location.CityName, result.CityName);
        Assert.AreEqual(location.CountryName, result.CountryName);
        Assert.AreEqual(location.ZipCode, result.ZipCode);
        Assert.AreEqual(location.Latitude, result.Latitude);
        Assert.AreEqual(location.Longitude, result.Longitude);
    }

    [Test]
    public async Task EditLocationAsync_ShouldUpdateAndReturnTrue()
    {
        var model = new EditLocationViewModel
        {
            Id = Guid.NewGuid(),
            Address = "NewAddress",
            CityName = "NewCity",
            CountryName = "NewCountry",
            ZipCode = "54321",
            Latitude = 15.0,
            Longitude = 25.0
        };

        var location = new Location
        {
            Id = model.Id,
            Address = "OldAddress",
            CityName = "OldCity",
            CountryName = "OldCountry",
            ZipCode = "11111",
            Latitude = 5.0,
            Longitude = 10.0
        };

        _locationRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Location, bool>>>()))
            .ReturnsAsync(location);

        _locationRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Location>()))
            .ReturnsAsync(true);

        var result = await _locationService.EditLocationAsync(model);

        Assert.IsTrue(result);

        // Check that location properties were updated correctly
        Assert.AreEqual(model.Address, location.Address);
        Assert.AreEqual(model.CityName, location.CityName);
        Assert.AreEqual(model.CountryName, location.CountryName);
        Assert.AreEqual(model.ZipCode, location.ZipCode);
        Assert.AreEqual(model.Latitude, location.Latitude);
        Assert.AreEqual(model.Longitude, location.Longitude);

        _locationRepositoryMock.Verify(r => r.UpdateAsync(location), Times.Once);
    }

    [Test]
    public async Task GetAllLocationsAsync_ShouldReturnPaginatedList()
    {
        var locations = new List<Location>
        {
            new Location
            {
                Id = Guid.NewGuid(),
                Address = "Addr1",
                CityName = "City1",
                CountryName = "Country1",
                ZipCode = "11111",
                Latitude = 10.0,
                Longitude = 20.0
            },
            new Location
            {
                Id = Guid.NewGuid(),
                Address = "Addr2",
                CityName = "City2",
                CountryName = "Country2",
                ZipCode = "22222",
                Latitude = 30.0,
                Longitude = 40.0
            }
        };

        _locationRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(locations);

        int page = 1;
        int pageSize = 10;

        var result = await _locationService.GetAllLocationsAsync(page, pageSize);

        Assert.IsNotNull(result);
        Assert.AreEqual(locations.Count, result.Count);
        Assert.AreEqual(locations[0].CityName, result[0].CityName);
        Assert.AreEqual(locations[1].CityName, result[1].CityName);
    }
}