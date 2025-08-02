using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                int pageSize = 10;
                var genres = await _genreService.GetGenreListAsync();
                var model = PaginatedList<GenreViewModel>.CreateFromList(genres, page, pageSize);
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
                TempData["ErrorMessage"] = "An Error occured while trying to fetch your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CreateGenreViewModel();
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
        public async Task<IActionResult> Create(CreateGenreViewModel model)
        {
            try
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
        public async Task<IActionResult> CreateInline([FromBody] CreateGenreViewModel model)
        {
            try
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
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {

                var model = await _genreService.GetGenreToEditAsync(id);
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
        public async Task<IActionResult> Edit(EditGenreViewModel model)
        {
            try
            {
                bool isUpdated = false;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                isUpdated = await _genreService.EditGenreAsync(model);
                if (!isUpdated)
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                    return View(model);
                }

                TempData["SuccessMessage"] = "Successfully updated genre!";
                return RedirectToAction("Index", "Genre", new { area = "Admin" });
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
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
