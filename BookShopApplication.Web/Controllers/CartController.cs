using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookShopApplication.Web.ViewModels.Cart;

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
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var model = await _service.DisplayAllCartItemsAsync(userId);
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your cart data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(Guid bookId)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while processing your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveById(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid bookId)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MoveToWishlistById(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(ICollection<CartItemViewModel> models)
        {
            try
            {

                var isSuccessful = await _service.PurchaseBooksAsync(models);

                if (isSuccessful)
                {
                    TempData["SuccessMessage"] = "Successfully purchased items in your cart!";
                }
                else
                {
                    TempData["ErrorMessage"] = "An Error occured while attempting to purchase the items!";
                }

                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrWhiteSpace(referer))
                {
                    return Redirect(referer);
                }

                // Fallback in case referrer is missing
                return RedirectToAction("Index", "Book");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your purchase.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

    }
}
