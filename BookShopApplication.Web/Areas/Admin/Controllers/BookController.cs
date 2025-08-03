using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;

        public BookController(IBookService bookService, IGenreService genreService)
        {
            _bookService = bookService;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                int pageSize = 10;
                var models = await _bookService.DisplayAllBooksAsync(page,pageSize);
                return View(models);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create(Guid shopId)
        {
            try
            {
                var genresModel = await _genreService.GetGenreListAsync();

                var model = new CreateBookViewModel
                {
                    Genres = genresModel.Select(g => new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    }).ToList(),
                    ShopId = shopId
                };
                return View(model);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    // Reload genres in case of validation failure
                    var genresModel = await _genreService.GetGenreListAsync();
                    model.Genres = genresModel.Select(g => new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    }).ToList();

                    return View(model);
                }

                await _bookService.CreateBookAsync(model);

                return RedirectToAction("Index", "Book", new { area = "Admin" });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, Guid shopId)
        {
            try
            {
                var model = await _bookService.GetBookToEdit(id, shopId);

                var genresModel = await _genreService.GetGenreListAsync();
                model.Genres = genresModel.Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                }).ToList();


                return View(model);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to process your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var genresModel = await _genreService.GetGenreListAsync();
                    model.Genres = genresModel.Select(g => new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    }).ToList();

                    return View(model);
                }

                bool success = await _bookService.EditBookAsync(model);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update the book.");
                    return View(model);
                }

                return RedirectToAction("Index", "Book", new { area = "Admin" });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
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
                Guid? userId = null;
                if (this.GetUserId() != null)
                {
                    userId = Guid.Parse(this.GetUserId()!);
                }

                var book = await _bookService.DisplayBookDetailsByIdAsync(userId, id);

                return View(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch book's details.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid shopId)
        {
            try
            {


                var isDeleted = await _bookService.DeleteBookAsync(id);
                if (!isDeleted)
                {
                    TempData["Error"] = "Failed to delete the shop.";
                    return RedirectToAction("Index", "Book", new { area = "Admin" });
                }

                TempData["Success"] = "Shop deleted successfully.";
                return RedirectToAction("Index", "Book", new { area = "Admin" });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 403 });
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
