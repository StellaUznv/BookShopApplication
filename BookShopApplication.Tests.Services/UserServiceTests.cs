using MockQueryable;

namespace BookShopApplication.Tests.Services;

using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopApplication.Services;
using BookShopApplication.Data.Models;

[TestFixture]
public class UserServiceTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _userManagerMock = MockUserManager();
        _userService = new UserService(_userManagerMock.Object);
    }

    private static Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null);
    }
    [Test]
    public async Task GetUsersAsync_ReturnsSelectListOfUsers()
    {
        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com" },
            new ApplicationUser { Id = Guid.NewGuid(), Email = "user2@example.com" }
        };

        _userManagerMock.Setup(m => m.Users).Returns(users.BuildMock());

        var result = await _userService.GetUsersAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().Text, Is.EqualTo("user1@example.com"));
    }
    [Test]
    public async Task GetUserByIdAsync_UserExists_ReturnsUser()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = Guid.Parse(userId) };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id.ToString(), Is.EqualTo(userId));
    }

    [Test]
    public async Task GetUserByIdAsync_UserNotFound_ReturnsNull()
    {
        _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

        var result = await _userService.GetUserByIdAsync("invalid-id");

        Assert.That(result, Is.Null);
    }
    [Test]
    public async Task UpdateUserNameAsync_UserExists_UpdatesAndReturnsSuccess()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = Guid.Parse(userId) };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var result = await _userService.UpdateUserNameAsync(userId, "John", "Doe");

        Assert.That(result.Succeeded, Is.True);
        Assert.That(user.FirstName, Is.EqualTo("John"));
        Assert.That(user.LastName, Is.EqualTo("Doe"));
    }

    [Test]
    public async Task UpdateUserNameAsync_UserNotFound_ReturnsFailedResult()
    {
        _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

        var result = await _userService.UpdateUserNameAsync("invalid-id", "John", "Doe");

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors.First().Description, Is.EqualTo("User not found"));
    }
    [Test]
    public async Task GetFullNameAsync_UserExists_ReturnsFullName()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = Guid.Parse(userId), FirstName = "Jane", LastName = "Smith" };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

        var result = await _userService.GetFullNameAsync(userId);

        Assert.That(result, Is.EqualTo("Jane Smith"));
    }

    [Test]
    public async Task GetFullNameAsync_UserNotFound_ReturnsNull()
    {
        _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

        var result = await _userService.GetFullNameAsync("invalid-id");

        Assert.That(result, Is.Null);
    }
}


