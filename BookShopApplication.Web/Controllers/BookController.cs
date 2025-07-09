using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _service;

        public BookController(IBookService service) 
        {
            this._service = service;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _service.DisplayAllBooksAsync();

            return View(books);
        }
    }
}
