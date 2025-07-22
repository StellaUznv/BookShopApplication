using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService service)
        {
            this._shopService= service;
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

    }
}
