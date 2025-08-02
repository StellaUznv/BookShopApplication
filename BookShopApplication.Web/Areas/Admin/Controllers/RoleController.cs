using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
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
        private readonly IRoleService _roleService;

        public RoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , IRoleService roleService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
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

                if (await _roleService.RoleExistsAsync(model))
                {
                    ModelState.AddModelError("", "Role already exists.");
                    return View(model);
                }

                var isCreated = await _roleService.CreateRoleAsync(model);
                if (isCreated.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in isCreated.Errors)
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
                var model = await _roleService.GetRoleToEditAsync(id);
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
                var result = await _roleService.EditRoleAsync(model);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }

                var currentUserId = _userManager.GetUserId(User);

                if (await _roleService.AssignRoleAsync(currentUserId!, model))
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
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
                var result = await _roleService.DeleteRoleAsync(id);

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
