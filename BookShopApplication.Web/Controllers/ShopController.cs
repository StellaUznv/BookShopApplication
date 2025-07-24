using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Identity;

namespace BookShopApplication.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShopController(IShopService service, UserManager<ApplicationUser> userManager)
        {
            this._shopService = service;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var models = await _shopService.DisplayAllShopsAsync();
            return View(models);
        }

        [HttpGet]
        public IActionResult Create(Guid locationId)
        {
            // Option 1: Use ViewBag to pass locationId to the view
            ViewBag.LocationId = locationId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShopViewModel model, [FromForm] Guid locationId)
        {
            bool isAdded = false;

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                isAdded = await _shopService.CreateShopAsync(model, userId, locationId);
            }
            else
            {
                return View(model);
            }

            if (isAdded)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null && !await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                }
                TempData["SuccessMessage"] = "Successfully created your shop!";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong!";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Guid? userId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userIdString, out var parsedId))
                {
                    userId = parsedId;
                }
            }
            var viewModel = await _shopService.DisplayShopAsync(id, userId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
