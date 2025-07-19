using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApplication.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService service)
        {
            this._shopService = service;
        }

        public async Task<IActionResult> Index()
        {
            Guid? userId = null;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(idValue))
                {
                    userId = Guid.Parse(idValue);
                }
            }

            var models = await _shopService.DisplayAllShopsAsync(userId);
            return View(models);
        }
    }
}
