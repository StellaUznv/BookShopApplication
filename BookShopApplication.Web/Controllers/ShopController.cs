using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookShopApplication.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BookShopApplication.Web.Controllers
{
    [AllowAnonymous]
    public class ShopController : BaseController
    {
        private readonly IShopService _shopService;
        private readonly IRoleService _roleService;

        public ShopController(IShopService service, IRoleService roleService)
        {
            this._shopService = service;
            _roleService = roleService;
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

                var userId = Guid.Parse(this.GetUserId()!);

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
                    if ( await _roleService.AssignManagerRoleAsync(userId.ToString()))
                    {
                        TempData["SuccessMessage"] = "Successfully created your shop!";
                    }
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

                Guid? userId = Guid.Parse(this.GetUserId());

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
