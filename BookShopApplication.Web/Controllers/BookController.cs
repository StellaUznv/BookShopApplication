using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApplication.Web.Controllers
{
    [AllowAnonymous]
    public class BookController : BaseController
    {
        private readonly IBookService _service;

        public BookController(IBookService service) 
        {
            this._service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                Guid? userId = Guid.Parse(this.GetUserId());

                var books = await _service.DisplayAllBooksAsync(userId);
                return View(books);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured trying to fetch books data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }


        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                Guid? userId = Guid.Parse(this.GetUserId());

                var book = await _service.DisplayBookDetailsByIdAsync(userId, id);

                return View(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to fetch book's details.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }


        }
        
    }
}
