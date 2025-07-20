using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService service)
        {
            this._shopService= service;
        }

        public async Task<IActionResult> ManagedShops()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var models = await _shopService.GetManagedShopsAsync(userId);
            return View(models);
        }
    }
}
