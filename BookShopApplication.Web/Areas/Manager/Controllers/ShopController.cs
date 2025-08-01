using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Book;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookShopApplication.Data.Models;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ShopController(IShopService service, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this._shopService = service;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ManagedShops()
        {
            try
            {

                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var models = await _shopService.GetManagedShopsAsync(userId);
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
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your page.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {

                var model = await _shopService.GetShopToEditAsync(id);
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
        public async Task<IActionResult> Edit(EditShopViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var success = await _shopService.EditShopAsync(model);

                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update the shop.");
                    return View(model);
                }

                // Store LocationId in TempData
                TempData["LocationId"] = model.LocationId;

                // Redirect to Location/Edit (no ID in URL)
                return RedirectToAction("Edit", "Location", new { area = "Manager" });
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
        public async Task<IActionResult> DisplayBooks(Guid id)
        {
            try
            {

                var model = await _shopService.GetBooksByShopIdAsync(id);
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
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your page.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {

                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var success = await _shopService.DeleteShopAsync(id);

                if (!success)
                {
                    TempData["Error"] = "Failed to delete the shop.";
                    return RedirectToAction("ManagedShops");
                }

                bool hasRemainingShops = await _shopService.HasUserAnyShopsAsync(userId);

                if (!hasRemainingShops)
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());
                    if (user != null && await _userManager.IsInRoleAsync(user, "Manager"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Manager");

                        // ✅ Refresh authentication cookie
                        await _signInManager.RefreshSignInAsync(user);

                        TempData["Success"] = "Shop deleted successfully.";
                        //todo: Fix redirect!!!
                        return RedirectToAction("Index", "Shop", new { area = "" });

                    }
                }

                TempData["Success"] = "Shop deleted successfully.";

                return RedirectToAction("ManagedShops");

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
