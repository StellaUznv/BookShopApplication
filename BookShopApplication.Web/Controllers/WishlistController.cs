using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Controllers
{
    public class WishlistController : Controller
    {

        private readonly IWishlistService _service;

        public WishlistController(IWishlistService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid userId)
        {
            var model = await _service.DisplayWishlistItemsAsync(userId);
            return View(model);
        }
    }
}
