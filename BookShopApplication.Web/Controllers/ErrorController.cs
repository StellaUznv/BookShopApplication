using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public async Task<IActionResult> HttpStatusCodeHandler(int statusCode)
        {
            return statusCode switch
            {
                403 => View("403"),
                404 => View("404"),
                500 => View("500"),
                _ => View("Error")
            };
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
