﻿using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BookShopApplication.Web.Controllers
{
    [Authorize]
    public class WishlistController : BaseController
    {

        private readonly IWishlistService _service;

        public WishlistController(IWishlistService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var userId = Guid.Parse(this.GetUserId()!);

                int pageSize = 8;

                var model = await _service.DisplayWishlistItemsAsync(userId , page, pageSize);
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your wishlist.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid bookId)
        {
            try
            {

                var userId = Guid.Parse(this.GetUserId()!);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveById(Guid id)
        {
            try
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

                var userId = Guid.Parse(this.GetUserId()!);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
    }
}
