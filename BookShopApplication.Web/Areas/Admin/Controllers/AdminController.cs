using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var model = await _adminService.GetAdminDashboardData();
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
    }
}
