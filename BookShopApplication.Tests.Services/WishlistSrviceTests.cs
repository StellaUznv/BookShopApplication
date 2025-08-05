using System.Linq.Expressions;
using BookShopApplication.Data.Repository.Contracts;
using MockQueryable;

namespace BookShopApplication.Tests.Services;

using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Services;
using BookShopApplication.Data.Repository;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class WishlistServiceTests
{
    private Mock<IWishlistRepository> _wishlistRepositoryMock;
    private WishlistService _wishlistService;

    [SetUp]
    public void Setup()
    {
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _wishlistService = new WishlistService(null, _wishlistRepositoryMock.Object);
    }
    [Test]
    public async Task DisplayWishlistItemsAsync_ReturnsPaginatedItems()
    {
        var userId = Guid.NewGuid();
        var wishlistItems = new List<WishlistItem>
        {
            new WishlistItem
            {
                Id = Guid.NewGuid(),
                BookId = Guid.NewGuid(),
                UserId = userId,
                Book = new Book { Title = "Test Book", Price = 15.99m, ImagePath = "/img.jpg" }
            }
        };

        _wishlistRepositoryMock
            .Setup(r => r.GetAllAttached())
            .Returns(wishlistItems.BuildMock());

        var result = await _wishlistService.DisplayWishlistItemsAsync(userId, 1, 10);

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Title, Is.EqualTo("Test Book"));
    }
    [Test]
    public async Task AddToWishlistAsync_AddsItem_WhenNotExists()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        _wishlistRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync(false);

        _wishlistRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WishlistItem>()))
            .ReturnsAsync(true);

        var result = await _wishlistService.AddToWishlistAsync(userId, bookId);

        Assert.IsTrue(result);
        _wishlistRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WishlistItem>()), Times.Once);
    }

    [Test]
    public async Task AddToWishlistAsync_ReturnsFalse_WhenAlreadyExists()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        _wishlistRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync(true);

        var result = await _wishlistService.AddToWishlistAsync(userId, bookId);

        Assert.IsFalse(result);
        _wishlistRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WishlistItem>()), Times.Never);
    }
    [Test]
    public async Task RemoveFromWishlistByIdAsync_RemovesItem_WhenFound()
    {
        var itemId = Guid.NewGuid();
        var wishlistItem = new WishlistItem { Id = itemId };

        _wishlistRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync(wishlistItem);

        _wishlistRepositoryMock
            .Setup(r => r.DeleteAsync(wishlistItem))
            .ReturnsAsync(true);

        var result = await _wishlistService.RemoveFromWishlistByIdAsync(itemId);

        Assert.IsTrue(result);
        _wishlistRepositoryMock.Verify(r => r.DeleteAsync(wishlistItem), Times.Once);
    }

    [Test]
    public async Task RemoveFromWishlistByIdAsync_ReturnsFalse_WhenItemNotFound()
    {
        _wishlistRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync((WishlistItem?)null);

        var result = await _wishlistService.RemoveFromWishlistByIdAsync(Guid.NewGuid());

        Assert.IsFalse(result);
    }
    [Test]
    public async Task RemoveFromWishlistAsync_RemovesItem_WhenFound()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var wishlistItem = new WishlistItem { Id = Guid.NewGuid(), UserId = userId, BookId = bookId };

        _wishlistRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync(wishlistItem);

        _wishlistRepositoryMock
            .Setup(r => r.DeleteAsync(wishlistItem))
            .ReturnsAsync(true);

        var result = await _wishlistService.RemoveFromWishlistAsync(userId, bookId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task RemoveFromWishlistAsync_ReturnsFalse_WhenItemNotFound()
    {
        _wishlistRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync((WishlistItem?)null);

        var result = await _wishlistService.RemoveFromWishlistAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.IsFalse(result);
    }
}

