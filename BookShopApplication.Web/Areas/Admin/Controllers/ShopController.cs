using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILocationService _locationService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ShopController(IShopService shopService, UserManager<ApplicationUser> userManager, ILocationService locationService,
            SignInManager<ApplicationUser> signInManager)
        {
            _shopService = shopService;
            _userManager = userManager;
            _locationService = locationService;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _shopService.GetAllShopsWithBooksAsync();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var shop = await _shopService.DisplayShopAsync(id,null);
            return View(shop);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var managers = await _userManager.Users.Select(u => new
            {
                Id = u.Id,
                Email = u.Email
            }).ToListAsync();
            var model = new CreateShopAsAdminViewModel
            {
                Managers = managers.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Email
                })
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateShopAsAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var managers = await _userManager.Users.Select(u => new
                {
                    Id = u.Id,
                    Email = u.Email
                }).ToListAsync();
                model.Managers = managers.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Email
                });
                return View(model);
            }

            bool locationCreated = false;
            
            locationCreated = await _locationService.CreateLocationAsync(model.Location);

            var shopModel = new CreateShopViewModel
            {
                Description = model.Description,
                Name = model.Name
            };

            bool shopCreated = false;

            if (locationCreated)
            {
                shopCreated = await _shopService.CreateShopAsync(shopModel, model.SelectedManagerId, model.Location.Id);
            }

            // Step 3: Assign Manager
            if (shopCreated)
            {
                var user = await _userManager.FindByIdAsync(model.SelectedManagerId.ToString());
                if (user != null && !await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    await _userManager.AddToRoleAsync(user, "Manager");

                    await _signInManager.RefreshSignInAsync(user);
                    TempData["SuccessMessage"] = "Successfully created your shop and assigned manager!";
                }
                
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong!";
            }

            return RedirectToAction("Index", "Shop", new { area = "Admin" });

        }

    }
}
