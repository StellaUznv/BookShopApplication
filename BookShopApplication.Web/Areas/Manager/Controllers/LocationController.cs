using BookShopApplication.Services;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Location;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            if (!TempData.ContainsKey("LocationId"))
            {
                return BadRequest("Missing Location ID.");
            }

            var locationId = Guid.Parse(TempData["LocationId"].ToString());

            var model = await _locationService.GetLocationToEditAsync(locationId);
            return View(model);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _locationService.EditLocationAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to update the shop.");
                return View(model);
            }

            return RedirectToAction("ManagedShops", "Shop");
        }
    }
}
