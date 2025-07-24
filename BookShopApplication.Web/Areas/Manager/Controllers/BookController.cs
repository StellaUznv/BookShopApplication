using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;

        public BookController(IBookService bookService, IGenreService genreService)
        {
            _bookService = bookService;
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid shopId)
        {
            var genresModel = await _genreService.GetGenreListAsync();

            var model = new CreateBookViewModel
            {
                Genres = genresModel.Select(g=> new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                }).ToList(),
                ShopId = shopId
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model)
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

            return RedirectToAction("DisplayBooks", "Shop", new { id = model.ShopId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, Guid shopId)
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

        [HttpPost]
        public async Task<IActionResult> Edit(EditBookViewModel model)
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
            return RedirectToAction("DisplayBooks", "Shop", new { id = model.ShopId });
        }
    }
}
