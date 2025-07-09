using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = await _service.DisplayWishlistItemsAsync(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var isAdded = await _service.AddToWishlistAsync(userId, bookId);

            if (isAdded)
            {
                return RedirectToAction("Index"); 
            }

            return NotFound();
        }
    }
}
