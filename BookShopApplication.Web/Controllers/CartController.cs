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
        [HttpPost]
        public async Task<IActionResult> RemoveById(Guid id)
        {
            var isRemoved = await _service.RemoveFromCartByIdAsync(id);

            if (isRemoved)
            {
                TempData["SuccessMessage"] = "Successfully removed book from your Cart!";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error occured while attempting to remove the item!";
            }

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer))
            {
                return Redirect(referer);
            }

            // Fallback in case referrer is missing
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var isRemoved = await _service.RemoveFromCartAsync(userId, bookId);

            if (isRemoved)
            {
                TempData["SuccessMessage"] = "Successfully removed book from your Cart!";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error occured while attempting to remove the item!";
            }

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer))
            {
                return Redirect(referer);
            }

            // Fallback in case referrer is missing
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> MoveToWishlistById(Guid id)
        {
            var isSuccessful = await _service.MoveToWishlistByIdAsync(id);

            if (isSuccessful)
            {
                TempData["SuccessMessage"] = "Successfully moved book to your Wishlist!";
            }
            else
            {
                TempData["ErrorMessage"] = "An Error occured while attempting to move the item!";
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
