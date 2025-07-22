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
        public async Task<IActionResult> Create()
        {
            var genresModel = await _genreService.GetGenreListAsync();

            var model = new CreateBookViewModel
            {
                Genres = genresModel.Select(g=> new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                }).ToList()
            };
            return View(model);
        }
    }
}
