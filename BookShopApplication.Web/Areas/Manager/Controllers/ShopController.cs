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
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var models = await _shopService.GetManagedShopsAsync(userId);
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _shopService.GetShopToEditAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditShopViewModel model)
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

        [HttpGet]
        public async Task<IActionResult> DisplayBooks(Guid id)
        {
            var model = await _shopService.GetBooksByShopIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
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
    }
}
