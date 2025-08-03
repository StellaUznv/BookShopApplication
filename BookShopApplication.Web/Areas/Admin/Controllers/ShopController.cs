using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShopController : BaseController
    {
        private readonly IShopService _shopService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public ShopController(IShopService shopService, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, IUserService userService, IRoleService roleService)
        {
            _shopService = shopService;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                int pageSize = 10;
                var models = await _shopService.GetAllShopsWithBooksAsync(page, pageSize);
                return View(models);
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
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {

                var shop = await _shopService.DisplayShopAsync(id, null);
                return View(shop);
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
                var model = new CreateShopAsAdminViewModel
                {
                    Managers = await _userService.GetUsersAsync()
                };

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
        public async Task<IActionResult> Create(CreateShopAsAdminViewModel model)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    model.Managers = await _userService.GetUsersAsync();
                    return View(model);
                }

                var shopCreated = await _shopService.CreateShopAsAdminAsync(model);

                if (shopCreated)
                {
                    if (await _roleService.AssignManagerRoleAsync(model.SelectedManagerId.ToString()))
                    {
                        TempData["SuccessMessage"] = "Successfully created your shop and assigned manager!";
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }

                return RedirectToAction("Index", "Shop", new { area = "Admin" });
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var model = await _shopService.GetShopToEditAsAdminAsync(id);

                model.Managers = await _userService.GetUsersAsync();
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
        public async Task<IActionResult> Edit(EditShopAsAdminViewModel model)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    model.Managers = await _userService.GetUsersAsync();
                    return View(model);
                }

                if (await _shopService.EditShopAsAdminAsync(model))
                {
                    if (await _roleService.AssignManagerRoleAsync(model.SelectedManagerId.ToString()))
                    {
                        TempData["SuccessMessage"] = "Successfully updated your shop and assigned manager!";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }

                return RedirectToAction("Index", "Shop", new { area = "Admin" });
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {


                var user = await _userManager.Users.FirstAsync(u => u.ManagedShops.Any(ms => ms.Id == id));
                var success = await _shopService.DeleteShopAsync(id);

                if (!success)
                {
                    TempData["Error"] = "Failed to delete the shop.";
                    return RedirectToAction("ManagedShops");
                }

                bool hasRemainingShops = await _shopService.HasUserAnyShopsAsync(user.Id);

                if (!hasRemainingShops)
                {
                    //var user = await _userManager.FindByIdAsync(userId.ToString());
                    if (user != null && await _userManager.IsInRoleAsync(user, "Manager"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Manager");

                        // ✅ Refresh authentication cookie
                        await _signInManager.RefreshSignInAsync(user);

                        TempData["Success"] = "Shop deleted successfully.";
                        //todo: Fix redirect!!!
                        return RedirectToAction("Index", "Shop", new { area = "Admin" });

                    }
                }

                TempData["Success"] = "Shop deleted successfully.";

                return RedirectToAction("Index", "Shop", new { area = "Admin" });

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
