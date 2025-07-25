﻿using BookShopApplication.Services.Contracts;
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
                TempData["SuccessMessage"] = "Successfully added book to your Wishlist!";
            }
            else
            {
                TempData["ErrorMessage"] = "Book is already in Wishlist!";
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
            var isRemoved = await _service.RemoveFromWishlistByIdAsync(id);

            if (isRemoved)
            {
                TempData["SuccessMessage"] = "Successfully removed book from your Wishlist!";
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

            var isRemoved = await _service.RemoveFromWishlistAsync(userId, bookId);

            if (isRemoved)
            {
                TempData["SuccessMessage"] = "Successfully removed book from your Wishlist!";
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
    }
}
