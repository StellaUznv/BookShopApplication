using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _locationService.GetAllLocationsAsync();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateLocationViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _locationService.CreateLocationAsync(model);
            return RedirectToAction("Index", "Location", new { area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //if (!TempData.ContainsKey("LocationId"))
            //{
            //    return BadRequest("Missing Location ID.");
            //}

            //var locationId = Guid.Parse(TempData["LocationId"].ToString());

            var model = await _locationService.GetLocationToEditAsync(id);
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

            return RedirectToAction("Index", "Location", new { area = "Admin" });
        }
    }
}
