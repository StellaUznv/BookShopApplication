using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApplication.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _service;

        public CartController(ICartService cartService)
        {
            _service = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model =await _service.DisplayAllCartItemsAsync(userId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var isAdded = await _service.AddToCartAsync(userId, bookId);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Successfully added book to your Cart!";
            }
            else
            {
                TempData["ErrorMessage"] = "Book is already in Cart!";
            }


            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer))
            {
                return Redirect(referer);
            }

            // Fallback in case referrer is missing
            return RedirectToAction("Index", "Book");
        }
    }
}
