using BookShopApplication.Data.Models;
using BookShopApplication.Web.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var roles = await _roleManager.Roles
                    .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                    .ToListAsync();
                return View(roles);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {

                var model = new CreateRoleViewModel();
                return View(model);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            try
            {


                if (!ModelState.IsValid)
                    return View(model);

                if (await _roleManager.RoleExistsAsync(model.Name))
                {
                    ModelState.AddModelError("", "Role already exists.");
                    return View(model);
                }

                var result = await _roleManager.CreateAsync(new ApplicationRole(model.Name));
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {

                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null) return NotFound();

                var model = new RoleEditViewModel
                {
                    Id = role.Id,
                    Name = role.Name!
                };

                foreach (var user in _userManager.Users.ToList())
                {
                    model.Users.Add(new UserRoleAssignmentViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName!,
                        IsAssigned = await _userManager.IsInRoleAsync(user, role.Name!)
                    });
                }

                return View(model);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            try
            {

                var role = await _roleManager.FindByIdAsync(model.Id.ToString());
                if (role == null) return NotFound();

                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }

                var currentUserId = _userManager.GetUserId(User);

                foreach (var userModel in model.Users)
                {
                    var user = await _userManager.FindByIdAsync(userModel.UserId.ToString());
                    if (user == null) continue;

                    var isInRole = await _userManager.IsInRoleAsync(user, role.Name!);

                    if (userModel.IsAssigned && !isInRole)
                        await _userManager.AddToRoleAsync(user, role.Name!);
                    else if (!userModel.IsAssigned && isInRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name!);
                        if (user.Id.ToString() == currentUserId)
                        {
                            await _signInManager.RefreshSignInAsync(user);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }
                    }

                }

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {

                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return NotFound();

                var result = await _roleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Failed to delete role.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Role deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

    }

}
