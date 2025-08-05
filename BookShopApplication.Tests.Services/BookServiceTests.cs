
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using BookShopApplication.Web.ViewModels.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;
using NUnit.Framework;

namespace BookShopApplication.Tests.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IWishlistRepository> _wishlistRepositoryMock;
        private Mock<ICartRepository> _cartRepositoryMock;
        private Mock<IBookInShopRepository> _bookInShopRepositoryMock;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _wishlistRepositoryMock = new Mock<IWishlistRepository>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _bookInShopRepositoryMock = new Mock<IBookInShopRepository>();

            _bookService = new BookService(
                _bookRepositoryMock.Object,
                _wishlistRepositoryMock.Object,
                _cartRepositoryMock.Object,
                _bookInShopRepositoryMock.Object
            );
        }

        [Test]
        public async Task CreateBookAsync_ShouldSaveBookAndBookInShop()
        {
            var model = new CreateBookViewModel
            {
                Title = "Test Book",
                Description = "Description",
                Author = "Author",
                Price = 10.0m,
                PagesNumber = 100,
                GenreId = Guid.NewGuid(),
                ShopId = Guid.NewGuid(),
                ImageFile = new FormFile(Stream.Null, 0, 0, "ImageFile", "test.jpg")
            };

            _bookRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync(true);
            _bookInShopRepositoryMock.Setup(r => r.AddAsync(It.IsAny<BookInShop>())).ReturnsAsync(true);

            var result = await _bookService.CreateBookAsync(model);

            Assert.IsTrue(result);
        }
        [Test]
        public async Task DisplayAllBooksAsync_NoUserId_ReturnsPaginatedBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", AuthorName = "Author 1", Price = 10m, Genre = new Genre { Name = "Genre1" }, ImagePath = "img1.jpg" },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", AuthorName = "Author 2", Price = 20m, Genre = new Genre { Name = "Genre2" }, ImagePath = "img2.jpg" }
            };

            var mockBooks = books.BuildMock();

            _bookRepositoryMock.Setup(r => r.GetAllAttached()).Returns(mockBooks);

            int page = 1, pageSize = 10;

            // Act
            var result = await _bookService.DisplayAllBooksAsync(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.That(result.All(b => !string.IsNullOrEmpty(b.Title)));
        }

        [Test]
        public async Task DisplayAllBooksAsync_WithUserId_ReturnsPaginatedBooksWithUserFlags()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", AuthorName = "Author 1", Price = 10m, Genre = new Genre { Name = "Genre1" }, ImagePath = "img1.jpg" },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", AuthorName = "Author 2", Price = 20m, Genre = new Genre { Name = "Genre2" }, ImagePath = "img2.jpg" }
            };

            var mockBooks = books.BuildMock();

            var wishlistIds = new List<Guid> { books.First().Id };
            var cartIds = new List<Guid> { books.Last().Id };

            _bookRepositoryMock.Setup(r => r.GetAllAttached()).Returns(mockBooks);
            _wishlistRepositoryMock.Setup(w => w.GetWishListedItemsIdsAsNoTrackingAsync(userId))
                .ReturnsAsync(wishlistIds);
            _cartRepositoryMock.Setup(c => c.GetCartItemsIdsAsNoTrackingAsync(userId))
                .ReturnsAsync(cartIds);

            int page = 1, pageSize = 10;

            // Act
            var result = await _bookService.DisplayAllBooksAsync(userId, page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            var firstBook = result.First(b => b.Id == books.First().Id);
            Assert.IsTrue(firstBook.IsInWishlist);
            Assert.IsFalse(firstBook.IsInCart);

            var secondBook = result.First(b => b.Id == books.Last().Id);
            Assert.IsFalse(secondBook.IsInWishlist);
            Assert.IsTrue(secondBook.IsInCart);
        }

        [Test]
        public async Task DisplayBookDetailsByIdAsync_NoUserId_ReturnsBookDetails()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                Title = "Book 1",
                AuthorName = "Author 1",
                Price = 15m,
                Genre = new Genre { Name = "Genre1" },
                ImagePath = "img.jpg",
                Description = "Description",
                PagesNumber = 123,
                BookInShops = new List<BookInShop>
                {
                    new BookInShop
                    {
                        Shop = new Shop { Id = Guid.NewGuid(), Name = "Shop1" },
                        ShopId = Guid.NewGuid()
                    }
                }
            };

            var books = new List<Book> { book };
            var mockBooks = books.BuildMock();

            _bookRepositoryMock.Setup(r => r.GetAllAttached())
                .Returns(mockBooks);

            // Act
            var result = await _bookService.DisplayBookDetailsByIdAsync(null, bookId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookId, result.Id);
            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Genre.Name, result.Genre);
            Assert.AreEqual(book.Description, result.Description);
            Assert.AreEqual(book.PagesNumber.ToString(), result.PagesNumber);
            Assert.AreEqual(1, result.AvailableInShops.Count);
            Assert.AreEqual("Shop1", result.AvailableInShops.First().Name);
        }

        [Test]
        public async Task DisplayBookDetailsByIdAsync_WithUserId_ReturnsBookDetailsWithFlags()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                Title = "Book 1",
                AuthorName = "Author 1",
                Price = 15m,
                Genre = new Genre { Name = "Genre1" },
                ImagePath = "img.jpg",
                Description = "Description",
                PagesNumber = 123,
                BookInShops = new List<BookInShop>
                {
                    new BookInShop
                    {
                        Shop = new Shop { Id = Guid.NewGuid(), Name = "Shop1" },
                        ShopId = Guid.NewGuid()
                    }
                }
            };

            var books = new List<Book> { book };
            var mockBooks = books.BuildMock();

            var wishlistIds = new List<Guid> { bookId };
            var cartIds = new List<Guid>();

            _bookRepositoryMock.Setup(r => r.GetAllAttached())
                .Returns(mockBooks);
            _wishlistRepositoryMock.Setup(w => w.GetWishListedItemsIdsAsNoTrackingAsync(userId))
                .ReturnsAsync(wishlistIds);
            _cartRepositoryMock.Setup(c => c.GetCartItemsIdsAsNoTrackingAsync(userId))
                .ReturnsAsync(cartIds);

            // Act
            var result = await _bookService.DisplayBookDetailsByIdAsync(userId, bookId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookId, result.Id);
            Assert.IsTrue(result.IsInWishlist);
            Assert.IsFalse(result.IsInCart);
        }

        [Test]
        public async Task GetBookToEdit_ReturnsEditBookViewModel()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var shopId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                AuthorName = "Author 1",
                Description = "Desc",
                GenreId = Guid.NewGuid(),
                ImagePath = "img.jpg",
                PagesNumber = 100,
                Price = 50m,
                Title = "Title"
            };

            _bookRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Book, bool>>>()))
                .ReturnsAsync(book);

            // Act
            var result = await _bookService.GetBookToEdit(bookId, shopId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookId, result.Id);
            Assert.AreEqual(shopId, result.ShopId);
            Assert.AreEqual(book.AuthorName, result.Author);
            Assert.AreEqual(book.Description, result.Description);
            Assert.AreEqual(book.GenreId, result.GenreId);
            Assert.AreEqual(book.ImagePath, result.ImagePath);
            Assert.AreEqual(book.PagesNumber, result.PagesNumber);
            Assert.AreEqual(book.Price, result.Price);
            Assert.AreEqual(book.Title, result.Title);
        }

        [Test]
        public async Task DeleteBookAsync_BookExists_DeletesBookAndBookInShops()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            var bookInShop = new BookInShop
            {
                BookId = bookId,
                ShopId = Guid.NewGuid()
            };

            var book = new Book
            {
                Id = bookId,
                BookInShops = new List<BookInShop> { bookInShop }
            };

            var books = new List<Book> { book };
            var mockBooks = books.BuildMock();

            _bookRepositoryMock.Setup(r => r.GetAllAttached())
                .Returns(mockBooks);

            _bookRepositoryMock.Setup(r => r.SoftDeleteAsync(book))
                .ReturnsAsync(true);

            _bookInShopRepositoryMock.Setup(r => r.SoftDeleteAsync(bookInShop))
                .ReturnsAsync(true);

            // Act
            var result = await _bookService.DeleteBookAsync(bookId);

            // Assert
            Assert.IsTrue(result);
            _bookInShopRepositoryMock.Verify(r => r.SoftDeleteAsync(bookInShop), Times.Once);
            _bookRepositoryMock.Verify(r => r.SoftDeleteAsync(book), Times.Once);
        }

        [Test]
        public async Task DeleteBookAsync_BookNotFound_ReturnsFalse()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            var emptyBooks = new List<Book>();
            var mockBooks = emptyBooks.BuildMock();

            _bookRepositoryMock.Setup(r => r.GetAllAttached())
                .Returns(mockBooks);

            // Act
            var result = await _bookService.DeleteBookAsync(bookId);

            // Assert
            Assert.IsFalse(result);
            _bookInShopRepositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<BookInShop>()), Times.Never);
            _bookRepositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Book>()), Times.Never);
        }

    [Test]
    public async Task DeleteBookAsync_PerformsSoftDeletes()
    {
        var bookId = Guid.NewGuid();
        var shopId = Guid.NewGuid();

        var book = new Book
        {
            Id = bookId,
            BookInShops = new List<BookInShop>
            {
                new BookInShop { BookId = bookId, ShopId = shopId }
            }
        };

        var books = new List<Book> { book }.BuildMock();

        _bookRepositoryMock.Setup(r => r.GetAllAttached()).Returns(books);
        _bookRepositoryMock.Setup(r => r.SoftDeleteAsync(book)).ReturnsAsync(true);
        _bookInShopRepositoryMock.Setup(r => r.SoftDeleteAsync(It.IsAny<BookInShop>())).ReturnsAsync(true);

        var result = await _bookService.DeleteBookAsync(bookId);

        Assert.IsTrue(result);
        _bookRepositoryMock.Verify(r => r.SoftDeleteAsync(book), Times.Once);
        _bookInShopRepositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<BookInShop>()), Times.Exactly(1));
    }
    }
}
