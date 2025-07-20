using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
