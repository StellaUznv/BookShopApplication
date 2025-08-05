using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Web.ViewModels.Search.DTOs;
using MockQueryable;

namespace BookShopApplication.Tests.Services;

using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopApplication.Services;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Data.Models;

using MockQueryable.Moq;

[TestFixture]
public class SearchServiceTests
{
    private Mock<IBookRepository> _bookRepositoryMock;
    private Mock<IShopRepository> _shopRepositoryMock;
    private SearchService _searchService;

    [SetUp]
    public void SetUp()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _shopRepositoryMock = new Mock<IShopRepository>();
        _searchService = new SearchService(_shopRepositoryMock.Object, _bookRepositoryMock.Object);
    }

    [Test]
    public async Task SearchAsync_FilterBook_ReturnsMatchingBooks()
    {
        var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "C# Basics", AuthorName = "John Doe", Genre = new Genre { Name = "Programming" } },
            new Book { Id = Guid.NewGuid(), Title = "Cooking 101", AuthorName = "Chef Mike", Genre = new Genre { Name = "Cooking" } }
        };

        var expected = new Book
        {
            Id = Guid.NewGuid(), Title = "C# Basics", AuthorName = "John Doe",
            Genre = new Genre { Name = "Programming" }
        };
        _bookRepositoryMock.Setup(r => r.GetAllAttached()).Returns(books.BuildMock());

        var result = await _searchService.SearchAsync("C#", "book", 1, 10);

        Assert.AreEqual(1, result.TotalResults);
        
    }
    [Test]
    public async Task SearchAsync_FilterShop_ReturnsMatchingShops()
    {
        var shops = new List<Shop>
        {
            new Shop { Id = Guid.NewGuid(), Name = "Tech Books", Description = "Books on programming", Location = new Location() },
            new Shop { Id = Guid.NewGuid(), Name = "Nature Books", Description = "Outdoor and nature themes", Location = new Location() }
        };
        var expected = new Shop
        {
            Id = Guid.NewGuid(), Name = "Tech Books", Description = "Books on programming", Location = new Location()
        };
        _shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(shops.BuildMock());

        var result = await _searchService.SearchAsync("tech", "shop", 1, 10);

        Assert.AreEqual(1, result.TotalResults);
        
    }
    [Test]
    public async Task SearchAsync_FilterLocation_ReturnsMatchingShops()
    {
        var shops = new List<Shop>
        {
            new Shop
            {
                Id = Guid.NewGuid(),
                Name = "City Central",
                Description = "Main store",
                Location = new Location { CityName = "Sofia", CountryName = "Bulgaria", Address = "Main Blvd" }
            }
        };
        var expected = new Shop
        {
            Id = Guid.NewGuid(),
            Name = "City Central",
            Description = "Main store",
            Location = new Location { CityName = "Sofia", CountryName = "Bulgaria", Address = "Main Blvd" }
        };
        _shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(shops.BuildMock());

        var result = await _searchService.SearchAsync("sofia", "location", 1, 10);

        Assert.AreEqual(1, result.TotalResults);
        
    }
    [Test]
    public async Task SearchAsync_FilterGenre_ReturnsMatchingBooks()
    {
        var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "Deep Learning", AuthorName = "AI Master", Genre = new Genre { Name = "Machine Learning" } },
            new Book { Id = Guid.NewGuid(), Title = "The Art of Cooking", AuthorName = "Chef Mike", Genre = new Genre { Name = "Cooking" } }
        };
        var expected = new SearchResultItemDto
        {
            Type = nameof(Book),
            Id = Guid.NewGuid(), Title = "Deep Learning", Subtitle = "AI Master",
            
        };
        _bookRepositoryMock.Setup(r => r.GetAllAttached()).Returns(books.BuildMock());

        var result = await _searchService.SearchAsync("machine", "genre", 1, 10);

        Assert.AreEqual(1, result.TotalResults);
       
    }
    [Test]
    public async Task SearchAsync_NoMatches_ReturnsEmptyResult()
    {
        _bookRepositoryMock.Setup(r => r.GetAllAttached()).Returns(new List<Book>().BuildMock());
        _shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(new List<Shop>().BuildMock());

        var result = await _searchService.SearchAsync("nonexistent", "all", 1, 10);

        Assert.AreEqual(0, result.TotalResults);
        Assert.IsEmpty(result.Results);
    }
}

