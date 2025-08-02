using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                int pageSize = 10;
                var models = await _locationService.GetAllLocationsAsync(page, pageSize);
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
        public async Task<IActionResult> Create()
        {
            try
            {

                var model = new CreateLocationViewModel();
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
        public async Task<IActionResult> Create(CreateLocationViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _locationService.CreateLocationAsync(model);
                return RedirectToAction("Index", "Location", new { area = "Admin" });
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
            //if (!TempData.ContainsKey("LocationId"))
            //{
            //    return BadRequest("Missing Location ID.");
            //}

            //var locationId = Guid.Parse(TempData["LocationId"].ToString());

            try
            {


                var model = await _locationService.GetLocationToEditAsync(id);
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
        public async Task<IActionResult> Edit(EditLocationViewModel model)
        {
            try
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
