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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ShopController(IShopService service, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._shopService = service;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var models = await _shopService.DisplayAllShopsAsync();
                return View(models);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An Error occured while trying to fetch the page.";
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet]
        public IActionResult Create(Guid locationId)
        {
            try
            {

                ViewBag.LocationId = locationId;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An Error occured while trying to fetch the page.";
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShopViewModel model, [FromForm] Guid locationId)
        {
            try
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

                        await _signInManager.RefreshSignInAsync(user);
                    }

                    TempData["SuccessMessage"] = "Successfully created your shop!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your shop data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
    }
}
