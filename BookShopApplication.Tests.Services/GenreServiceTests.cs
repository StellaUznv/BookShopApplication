using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using BookShopApplication.Web.ViewModels.Genre;
using MockQueryable;
using Moq;

namespace BookShopApplication.Tests.Services;

[TestFixture]
public class GenreServiceTests
{
    private Mock<IGenreRepository> _genreRepositoryMock;
    private Mock<IBookRepository> _bookRepositoryMock;
    private GenreService _genreService;

    [SetUp]
    public void SetUp()
    {
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _bookRepositoryMock = new Mock<IBookRepository>();
        _genreService = new GenreService(_genreRepositoryMock.Object, _bookRepositoryMock.Object);
    }

    [Test]
    public async Task GetGenreListAsync_ShouldReturnGenreViewModelList()
    {
        var genres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Genre1", Description = "Desc1" },
            new Genre { Id = Guid.NewGuid(), Name = "Genre2", Description = "Desc2" }
        };

        _genreRepositoryMock.Setup(r => r.GetAllAttached()).Returns(genres.BuildMock());

        var result = await _genreService.GetGenreListAsync();

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(g => g.Name == "Genre1"));
        Assert.IsTrue(result.Any(g => g.Name == "Genre2"));
    }

    [Test]
    public async Task GetAllGenresAsync_ShouldReturnAllGenres()
    {
        var genres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Genre1" },
            new Genre { Id = Guid.NewGuid(), Name = "Genre2" }
        };

        _genreRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(genres);

        var result = await _genreService.GetAllGenresAsync();

        Assert.AreEqual(2, result.Count());
    }

    [Test]
    public async Task AddNewGenreAsync_WhenGenreDoesNotExist_ShouldAddAndReturnTrue()
    {
        var model = new CreateGenreViewModel { Id = Guid.NewGuid(), Name = "NewGenre", Description = "NewDesc" };
        _genreRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Genre>())).ReturnsAsync(true);
        _genreRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Genre>());

        var result = await _genreService.AddNewGenreAsync(model);

        Assert.IsTrue(result);
        _genreRepositoryMock.Verify(r => r.AddAsync(It.Is<Genre>(g => g.Name == model.Name && g.Description == model.Description)), Times.Once);
    }

    [Test]
    public async Task AddNewGenreAsync_WhenGenreExistsAndIsDeleted_ShouldReactivateAndReturnTrue()
    {
        var existingGenre = new Genre { Id = Guid.NewGuid(), Name = "ExistingGenre", IsDeleted = true };
        var model = new CreateGenreViewModel { Id = Guid.NewGuid(), Name = "ExistingGenre", Description = "Desc" };

        _genreRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Genre> { existingGenre });
        _genreRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Genre>())).ReturnsAsync(true);

        var result = await _genreService.AddNewGenreAsync(model);

        Assert.IsTrue(result);
        Assert.IsFalse(existingGenre.IsDeleted);
        _genreRepositoryMock.Verify(r => r.UpdateAsync(existingGenre), Times.Once);
    }

    [Test]
    public async Task AddNewGenreAsync_WhenGenreExistsAndIsNotDeleted_ShouldReturnFalse()
    {
        var existingGenre = new Genre { Id = Guid.NewGuid(), Name = "ExistingGenre", IsDeleted = false };
        var model = new CreateGenreViewModel { Id = Guid.NewGuid(), Name = "ExistingGenre", Description = "Desc" };

        _genreRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Genre> { existingGenre });

        var result = await _genreService.AddNewGenreAsync(model);

        Assert.IsFalse(result);
        _genreRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Never);
        _genreRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Genre>()), Times.Never);
    }

    [Test]
    public async Task GetGenreToEditAsync_ShouldReturnEditGenreViewModel()
    {
        var genreId = Guid.NewGuid();
        var genre = new Genre { Id = genreId, Name = "GenreName", Description = "Desc" };
        _genreRepositoryMock.Setup(r => r.GetByIdAsync(genreId)).ReturnsAsync(genre);

        var result = await _genreService.GetGenreToEditAsync(genreId);

        Assert.AreEqual(genreId, result.Id);
        Assert.AreEqual("GenreName", result.Name);
        Assert.AreEqual("Desc", result.Description);
    }

    [Test]
    public async Task EditGenreAsync_ShouldCallUpdateAsyncAndReturnTrue()
    {
        var model = new EditGenreViewModel { Id = Guid.NewGuid(), Name = "NewName", Description = "NewDesc" };
        _genreRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Genre>())).ReturnsAsync(true);

        var result = await _genreService.EditGenreAsync(model);

        Assert.IsTrue(result);
        _genreRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Genre>(g => g.Id == model.Id && g.Name == model.Name && g.Description == model.Description)), Times.Once);
    }

    [Test]
    public async Task DeleteGenreAsync_WhenBooksExist_ShouldReturnFalse()
    {
        var genreId = Guid.NewGuid();
        var genre = new Genre { Id = genreId };
        _genreRepositoryMock.Setup(r => r.GetByIdAsync(genreId)).ReturnsAsync(genre);
        _bookRepositoryMock.Setup(r => r.AnyAsync(b => b.GenreId == genreId)).ReturnsAsync(true);

        var result = await _genreService.DeleteGenreAsync(genreId);

        Assert.IsFalse(result);
        _genreRepositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Genre>()), Times.Never);
    }

    [Test]
    public async Task DeleteGenreAsync_WhenNoBooksExist_ShouldSoftDeleteAndReturnTrue()
    {
        var genreId = Guid.NewGuid();
        var genre = new Genre { Id = genreId };
        _genreRepositoryMock.Setup(r => r.GetByIdAsync(genreId)).ReturnsAsync(genre);
        _bookRepositoryMock.Setup(r => r.AnyAsync(b => b.GenreId == genreId)).ReturnsAsync(false);
        _genreRepositoryMock.Setup(r => r.SoftDeleteAsync(genre)).ReturnsAsync(true);

        var result = await _genreService.DeleteGenreAsync(genreId);

        Assert.IsTrue(result);
        _genreRepositoryMock.Verify(r => r.SoftDeleteAsync(genre), Times.Once);
    }
}