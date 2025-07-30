using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetGenreListAsync();
            return View(genres);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateGenreViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreViewModel model)
        {
            bool isCreated = false;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            isCreated = await _genreService.AddNewGenreAsync(model);
            if (!isCreated)
            {
                TempData["ErrorMessage"] = "Something went wrong!";
                return View(model);
            }
            TempData["SuccessMessage"] = "Successfully created new genre!";

            return RedirectToAction("Index", "Genre", new { area = "Admin" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInline([FromBody] CreateGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "Invalid input." });
            }

            var success = await _genreService.AddNewGenreAsync(model);

            if (!success)
            {
                return Json(new { success = false, error = "Could not create genre." });
            }

            return Json(new { success = true, name = model.Name, id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _genreService.GetGenreToEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGenreViewModel model)
        {
            bool isUpdated = false;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            isUpdated = await _genreService.EditGenreAsync(model);
            if(!isUpdated)
            {
                TempData["ErrorMessage"] = "Something went wrong!";
                return View(model);
            }

            TempData["SuccessMessage"] = "Successfully updated genre!";
            return RedirectToAction("Index", "Genre", new { area = "Admin" });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isDeleted = await _genreService.DeleteGenreAsync(id);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] = "The Genre is used by a book!";
                return RedirectToAction("Index", "Genre", new { area = "Admin" });
            }
            TempData["SuccessMessage"] = "Successfully deleted genre!";
            return RedirectToAction("Index", "Genre", new { area = "Admin" });
        }
    }
}
