using System.Linq.Expressions;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Web.ViewModels.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;

namespace BookShopApplication.Tests.Services;

[TestFixture]
public class CartServiceTests
{
    private Mock<ICartRepository> _cartRepoMock;
    private Mock<IWishlistRepository> _wishlistRepoMock;
    private Mock<IPurchaseItemUserRepository> _purchaseRepoMock;

    private CartService _service;

    [SetUp]
    public void Setup()
    {
        _cartRepoMock = new Mock<ICartRepository>();
        _wishlistRepoMock = new Mock<IWishlistRepository>();
        _purchaseRepoMock = new Mock<IPurchaseItemUserRepository>();

        _service = new CartService(_cartRepoMock.Object, _wishlistRepoMock.Object, _purchaseRepoMock.Object);
    }

    [Test]
    public async Task DisplayAllCartItemsAsync_ReturnsCorrectCartViewModel()
    {
        var userId = Guid.NewGuid();

        var cartItems = new List<CartItem>
        {
            new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = Guid.NewGuid(),
                Quantity = 2,
                Book = new Book { Title = "Test Book 1", Price = 10.50m, ImagePath = "img1.jpg" }
            },
            new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = Guid.NewGuid(),
                Quantity = 1,
                Book = new Book { Title = "Test Book 2", Price = 5.00m, ImagePath = "img2.jpg" }
            }
        };

        _cartRepoMock.Setup(r => r.GetAllAttached()).Returns(cartItems.BuildMock());

        var result = await _service.DisplayAllCartItemsAsync(userId, page: 1, pageSize: 10);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Items.Count);
        // Total = (10.50 * 2) + (5.00 * 1) = 26.00
        Assert.AreEqual(26.00m, result.Total);

        // Validate item data
        var firstItem = result.Items.First();
        Assert.AreEqual("Test Book 1", firstItem.Title);
        Assert.AreEqual("10.50", firstItem.Price);
        Assert.AreEqual(2, firstItem.Quantity);
    }

    [Test]
    public async Task AddToCartAsync_ReturnsFalse_WhenItemAlreadyExists()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        _cartRepoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<CartItem, bool>>>())).ReturnsAsync(true);

        var result = await _service.AddToCartAsync(userId, bookId);

        Assert.IsFalse(result);
        _cartRepoMock.Verify(r => r.AddAsync(It.IsAny<CartItem>()), Times.Never);
    }

    [Test]
    public async Task AddToCartAsync_RemovesFromWishlist_IfExists()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        _cartRepoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<CartItem, bool>>>())).ReturnsAsync(false);
        _wishlistRepoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>())).ReturnsAsync(true);
        _wishlistRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<WishlistItem, bool>>>()))
            .ReturnsAsync(new WishlistItem { Id = Guid.NewGuid(), UserId = userId, BookId = bookId });
        _wishlistRepoMock.Setup(r => r.DeleteAsync(It.IsAny<WishlistItem>())).ReturnsAsync(true);
        _cartRepoMock.Setup(r => r.AddAsync(It.IsAny<CartItem>())).ReturnsAsync(true);

        var result = await _service.AddToCartAsync(userId, bookId);

        Assert.IsTrue(result);
        _wishlistRepoMock.Verify(r => r.DeleteAsync(It.IsAny<WishlistItem>()), Times.Once);
        _cartRepoMock.Verify(r => r.AddAsync(It.IsAny<CartItem>()), Times.Once);
    }

    [Test]
    public async Task RemoveFromCartByIdAsync_ReturnsTrue_WhenItemRemoved()
    {
        var itemId = Guid.NewGuid();
        var cartItem = new CartItem { Id = itemId };

        _cartRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
            .ReturnsAsync(cartItem);
        _cartRepoMock.Setup(r => r.DeleteAsync(cartItem))
            .ReturnsAsync(true);

        var result = await _service.RemoveFromCartByIdAsync(itemId);

        Assert.IsTrue(result);
        _cartRepoMock.Verify(r => r.DeleteAsync(cartItem), Times.Once);
    }

    [Test]
    public async Task RemoveFromCartAsync_ReturnsTrue_WhenItemRemoved()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var cartItem = new CartItem { UserId = userId, BookId = bookId };

        _cartRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
            .ReturnsAsync(cartItem);
        _cartRepoMock.Setup(r => r.DeleteAsync(cartItem))
            .ReturnsAsync(true);

        var result = await _service.RemoveFromCartAsync(userId, bookId);

        Assert.IsTrue(result);
        _cartRepoMock.Verify(r => r.DeleteAsync(cartItem), Times.Once);
    }

    [Test]
    public async Task MoveToWishlistByIdAsync_ReturnsTrue_WhenMovedSuccessfully()
    {
        var itemId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var cartItem = new CartItem { Id = itemId, UserId = userId, BookId = bookId };

        _cartRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
            .ReturnsAsync(cartItem);
        _cartRepoMock.Setup(r => r.DeleteAsync(cartItem))
            .ReturnsAsync(true);
        _wishlistRepoMock.Setup(r => r.AddAsync(It.IsAny<WishlistItem>()))
            .ReturnsAsync(true);

        var result = await _service.MoveToWishlistByIdAsync(itemId);

        Assert.IsTrue(result);
        _cartRepoMock.Verify(r => r.DeleteAsync(cartItem), Times.Once);
        _wishlistRepoMock.Verify(r => r.AddAsync(It.IsAny<WishlistItem>()), Times.Once);
    }

    [Test]
    public async Task PurchaseBooksAsync_ReturnsTrue_WhenAllProcessed()
    {
        var userId = Guid.NewGuid();

        var cartItemId1 = Guid.NewGuid();
        var cartItemId2 = Guid.NewGuid();

        var books = new List<CartItemViewModel>
        {
            new CartItemViewModel { Id = cartItemId1, UserId = userId },
            new CartItemViewModel { Id = cartItemId2, UserId = userId }
        };

        _cartRepoMock.Setup(r => r.GetByIdAsync(cartItemId1))
            .ReturnsAsync(new CartItem { Id = cartItemId1 });
        _cartRepoMock.Setup(r => r.GetByIdAsync(cartItemId2))
            .ReturnsAsync(new CartItem { Id = cartItemId2 });

        _cartRepoMock.Setup(r => r.SoftDeleteAsync(It.IsAny<CartItem>())).ReturnsAsync(true);

        _purchaseRepoMock.Setup(r => r.AddRangeAsync(It.IsAny<PurchaseItemUser[]>())).ReturnsAsync(true);

        var result = await _service.PurchaseBooksAsync(books);

        Assert.IsTrue(result);
        _cartRepoMock.Verify(r => r.SoftDeleteAsync(It.IsAny<CartItem>()), Times.Exactly(2));
        _purchaseRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<PurchaseItemUser[]>()), Times.Once);
    }

    [Test]
    public async Task PurchaseBooksAsync_ReturnsFalse_IfSoftDeleteFails()
    {
        var userId = Guid.NewGuid();
        var cartItemId = Guid.NewGuid();

        var books = new List<CartItemViewModel>
        {
            new CartItemViewModel { Id = cartItemId, UserId = userId }
        };

        _cartRepoMock.Setup(r => r.GetByIdAsync(cartItemId))
            .ReturnsAsync(new CartItem { Id = cartItemId });

        _cartRepoMock.Setup(r => r.SoftDeleteAsync(It.IsAny<CartItem>())).ReturnsAsync(false);

        var result = await _service.PurchaseBooksAsync(books);

        Assert.IsFalse(result);
        _cartRepoMock.Verify(r => r.SoftDeleteAsync(It.IsAny<CartItem>()), Times.Once);
        _purchaseRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<PurchaseItemUser[]>()), Times.Never);
    }
}