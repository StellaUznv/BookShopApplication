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
    }
}
