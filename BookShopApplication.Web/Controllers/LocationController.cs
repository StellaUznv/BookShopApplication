using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Location;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BookShopApplication.Web.Controllers
{
    [Authorize]
    public class LocationController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            this._locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateToShop()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to get to the page.";
                return RedirectToAction("HttpStatusCodeHandler", "Error", new{statusCode = 404});
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateToShop(CreateLocationViewModel model)
        {
            try
            {

                bool isAdded = false;

                Guid? userId = Guid.Parse(this.GetUserId());

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "Please login first!";
                    return RedirectToAction("Index", "Shop");
                }

                if (ModelState.IsValid)
                {
                    isAdded = await _locationService.CreateLocationAsync(model);
                }
                else
                {
                    return View(model);
                }

                if (isAdded)
                {
                    TempData["SuccessMessage"] = "Successfully created the location of your shop!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }

                return RedirectToAction("Create", "Shop", new { locationId = model.Id });
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
