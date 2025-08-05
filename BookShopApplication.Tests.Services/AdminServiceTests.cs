using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;

namespace BookShopApplication.Tests.Services;

[TestFixture]
public class AdminServiceTests
{
    private Mock<IBookRepository> _bookRepositoryMock;
    private Mock<IShopRepository> _shopRepositoryMock;
    private Mock<IGenreRepository> _genreRepositoryMock;
    private Mock<ILocationRepository> _locationRepositoryMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<ApplicationRole>> _roleManagerMock;

    private AdminService _adminService;

    [SetUp]
    public void SetUp()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _shopRepositoryMock = new Mock<IShopRepository>();
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
            Mock.Of<IRoleStore<ApplicationRole>>(), null, null, null, null);

        _adminService = new AdminService(
            _bookRepositoryMock.Object,
            _shopRepositoryMock.Object,
            _genreRepositoryMock.Object,
            _locationRepositoryMock.Object,
            _userManagerMock.Object,
            _roleManagerMock.Object);
    }
        [Test]
    public async Task GetAdminDashboardData_ReturnsCorrectCounts()
    {
        // Arrange
        _bookRepositoryMock.Setup(r => r.GetCountAsync()).ReturnsAsync(10);
        _genreRepositoryMock.Setup(r => r.GetCountAsync()).ReturnsAsync(5);
        _locationRepositoryMock.Setup(r => r.GetCountAsync()).ReturnsAsync(3);
        _shopRepositoryMock.Setup(r => r.GetCountAsync()).ReturnsAsync(7);

        var roles = new List<ApplicationRole> { new ApplicationRole(), new ApplicationRole() }.BuildMock();
        var users = new List<ApplicationUser> { new ApplicationUser(), new ApplicationUser(), new ApplicationUser() }.BuildMock();

        var roleDbSetMock = new Mock<DbSet<ApplicationRole>>();
        roleDbSetMock.As<IQueryable<ApplicationRole>>().Setup(m => m.Provider).Returns(roles.Provider);
        roleDbSetMock.As<IQueryable<ApplicationRole>>().Setup(m => m.Expression).Returns(roles.Expression);
        roleDbSetMock.As<IQueryable<ApplicationRole>>().Setup(m => m.ElementType).Returns(roles.ElementType);
        roleDbSetMock.As<IQueryable<ApplicationRole>>().Setup(m => m.GetEnumerator()).Returns(roles.GetEnumerator());

        var userDbSetMock = new Mock<DbSet<ApplicationUser>>();
        userDbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(users.Provider);
        userDbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
        userDbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        userDbSetMock.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _roleManagerMock.Setup(r => r.Roles).Returns(roleDbSetMock.Object);
        _userManagerMock.Setup(u => u.Users).Returns(userDbSetMock.Object);

        // Act
        var result = await _adminService.GetAdminDashboardData();

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.TotalBooks, Is.EqualTo(10));
        Assert.That(result.TotalGenres, Is.EqualTo(5));
        Assert.That(result.TotalLocations, Is.EqualTo(3));
        Assert.That(result.TotalShops, Is.EqualTo(7));
        Assert.That(result.TotalRoles, Is.EqualTo(2));
        Assert.That(result.TotalUsers, Is.EqualTo(3));
    }
}

