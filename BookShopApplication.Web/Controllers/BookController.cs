using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApplication.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _service;

        public BookController(IBookService service) 
        {
            this._service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var books = await _service.DisplayAllBooksAsync(userId);

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var book = await _service.DisplayBookDetailsByIdAsync(userId, id);

            return View(book);
        }
    }
}
