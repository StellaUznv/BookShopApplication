using BookShopApplication.Data.Models;
using BookShopApplication.Services;
using BookShopApplication.Web.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;

namespace BookShopApplication.Tests.Services;

[TestFixture]
public class RoleServiceTests
{
    private Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private RoleService _roleService;

    [SetUp]
    public void SetUp()
    {
        // RoleManager mock setup
        var roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(roleStoreMock.Object, null, null, null, null);

        // UserManager mock setup
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        // SignInManager mock setup
        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            contextAccessor.Object,
            userPrincipalFactory.Object,
            null,
            null,
            null,
            null);

        _roleService = new RoleService(_roleManagerMock.Object, _userManagerMock.Object, _signInManagerMock.Object);
    }

    [Test]
    public async Task GetAllRolesAsync_ReturnsPaginatedRoles()
    {
        var roles = new List<ApplicationRole>
        {
            new ApplicationRole { Id = Guid.NewGuid(), Name = "Admin" },
            new ApplicationRole { Id = Guid.NewGuid(), Name = "User" }
        };

        _roleManagerMock.Setup(r => r.Roles).Returns(roles.BuildMock());

        var result = await _roleService.GetAllRolesAsync(1, 10);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Admin", result[0].Name);
        Assert.AreEqual("User", result[1].Name);
    }

    [Test]
    public async Task RoleExistsAsync_ReturnsTrueIfExists()
    {
        _roleManagerMock.Setup(r => r.RoleExistsAsync("Admin")).ReturnsAsync(true);

        var result = await _roleService.RoleExistsAsync(new CreateRoleViewModel { Name = "Admin" });

        Assert.IsTrue(result);
    }

    [Test]
    public async Task CreateRoleAsync_ReturnsSuccessIdentityResult()
    {
        var identityResult = IdentityResult.Success;
        _roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<ApplicationRole>())).ReturnsAsync(identityResult);

        var result = await _roleService.CreateRoleAsync(new CreateRoleViewModel { Name = "NewRole" });

        Assert.IsTrue(result.Succeeded);
    }

    [Test]
    public async Task GetRoleToEditAsync_ReturnsRoleEditViewModelWithUsers()
    {
        var roleId = Guid.NewGuid();
        var role = new ApplicationRole { Id = roleId, Name = "Admin" };

        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = Guid.NewGuid(), UserName = "User1" },
            new ApplicationUser { Id = Guid.NewGuid(), UserName = "User2" }
        }.AsQueryable();

        _roleManagerMock.Setup(r => r.FindByIdAsync(roleId.ToString())).ReturnsAsync(role);
        _userManagerMock.Setup(u => u.Users).Returns(users);

        _userManagerMock.Setup(u => u.IsInRoleAsync(users.ToArray()[0], role.Name)).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.IsInRoleAsync(users.ToArray()[1], role.Name)).ReturnsAsync(false);

        var result = await _roleService.GetRoleToEditAsync(roleId);

        Assert.AreEqual(roleId, result.Id);
        Assert.AreEqual("Admin", result.Name);
        Assert.AreEqual(2, result.Users.Count);
        Assert.IsTrue(result.Users.First().IsAssigned);
        Assert.IsFalse(result.Users.Last().IsAssigned);
    }

    [Test]
    public void GetRoleToEditAsync_ThrowsIfRoleNotFound()
    {
        var roleId = Guid.NewGuid();
        _roleManagerMock.Setup(r => r.FindByIdAsync(roleId.ToString())).ReturnsAsync((ApplicationRole)null);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleService.GetRoleToEditAsync(roleId));
    }

    [Test]
    public async Task EditRoleAsync_ReturnsSuccessIdentityResult()
    {
        var model = new RoleEditViewModel { Id = Guid.NewGuid(), Name = "UpdatedRole" };
        var role = new ApplicationRole { Id = model.Id, Name = "OldRole" };

        _roleManagerMock.Setup(r => r.FindByIdAsync(model.Id.ToString())).ReturnsAsync(role);
        _roleManagerMock.Setup(r => r.UpdateAsync(role)).ReturnsAsync(IdentityResult.Success);

        var result = await _roleService.EditRoleAsync(model);

        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual("UpdatedRole", role.Name);
    }

    [Test]
    public void EditRoleAsync_ThrowsIfRoleNotFound()
    {
        var model = new RoleEditViewModel { Id = Guid.NewGuid(), Name = "UpdatedRole" };

        _roleManagerMock.Setup(r => r.FindByIdAsync(model.Id.ToString())).ReturnsAsync((ApplicationRole)null);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleService.EditRoleAsync(model));
    }

    [Test]
    public async Task AssignRoleAsync_ReturnsTrueOnSuccessfulRoleAssignmentOrRemoval()
    {
        // Arrange
        var currentUserId = Guid.NewGuid().ToString();
        var roleName = "Admin";

        var user1 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "User1" };
        var user2 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "User2" };

        var model = new RoleEditViewModel
        {
            Name = roleName,
            Users = new List<UserRoleAssignmentViewModel>
            {
                new UserRoleAssignmentViewModel { UserId = user1.Id, IsAssigned = true },  // needs to be added
                new UserRoleAssignmentViewModel { UserId = user2.Id, IsAssigned = false } // already has role
            }
        };

        _userManagerMock.Setup(u => u.FindByIdAsync(user1.Id.ToString())).ReturnsAsync(user1);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user1, roleName)).ReturnsAsync(false); // should be added
        _userManagerMock.Setup(u => u.AddToRoleAsync(user1, roleName)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _roleService.AssignRoleAsync(currentUserId, model);

        // Assert
        Assert.IsTrue(result); // because a role was added
        _userManagerMock.Verify(u => u.AddToRoleAsync(user1, roleName), Times.Once);
    }
    [Test]
    public async Task AssignRoleAsync_ReturnsTrueOnRoleRemoval_EvenIfNotCurrentUser()
    {
        var currentUserId = Guid.NewGuid().ToString();
        var roleName = "Manager";

        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "ToRemove" };

        var model = new RoleEditViewModel
        {
            Name = roleName,
            Users = new List<UserRoleAssignmentViewModel>
            {
                new UserRoleAssignmentViewModel { UserId = user.Id, IsAssigned = false }
            }
        };

        _userManagerMock.Setup(u => u.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, roleName)).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.RemoveFromRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

        var result = await _roleService.AssignRoleAsync(currentUserId, model);

        Assert.IsTrue(result); // returns true because removal happened
        _userManagerMock.Verify(u => u.RemoveFromRoleAsync(user, roleName), Times.Once);
        _signInManagerMock.Verify(s => s.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Test]
    public async Task DeleteRoleAsync_ReturnsSuccessIdentityResult()
    {
        var roleId = Guid.NewGuid().ToString();
        var role = new ApplicationRole { Id = Guid.Parse(roleId), Name = "RoleToDelete" };

        _roleManagerMock.Setup(r => r.FindByIdAsync(roleId)).ReturnsAsync(role);
        _roleManagerMock.Setup(r => r.DeleteAsync(role)).ReturnsAsync(IdentityResult.Success);

        var result = await _roleService.DeleteRoleAsync(roleId);

        Assert.IsTrue(result.Succeeded);
    }

    [Test]
    public void DeleteRoleAsync_ThrowsIfRoleNotFound()
    {
        var roleId = Guid.NewGuid().ToString();

        _roleManagerMock.Setup(r => r.FindByIdAsync(roleId)).ReturnsAsync((ApplicationRole)null);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleService.DeleteRoleAsync(roleId));
    }

    [Test]
    public async Task AssignManagerRoleAsync_AddsManagerRoleAndRefreshesSignIn()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = Guid.Parse(userId) };

        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, "Manager")).ReturnsAsync(false);
        _userManagerMock.Setup(u => u.AddToRoleAsync(user, "Manager")).ReturnsAsync(IdentityResult.Success);
        _signInManagerMock.Setup(s => s.RefreshSignInAsync(user)).Returns(Task.CompletedTask);

        var result = await _roleService.AssignManagerRoleAsync(userId);

        Assert.IsTrue(result);
        _userManagerMock.Verify(u => u.AddToRoleAsync(user, "Manager"), Times.Once);
        _signInManagerMock.Verify(s => s.RefreshSignInAsync(user), Times.Once);
    }

    [Test]
    public async Task AssignManagerRoleAsync_ReturnsFalseIfUserNullOrAlreadyInRole()
    {
        var userId = Guid.NewGuid().ToString();

        // User not found
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

        var result = await _roleService.AssignManagerRoleAsync(userId);
        Assert.IsFalse(result);

        // User found but already in role
        var user = new ApplicationUser { Id = Guid.Parse(userId) };
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, "Manager")).ReturnsAsync(true);

        result = await _roleService.AssignManagerRoleAsync(userId);
        Assert.IsFalse(result);
    }
}
