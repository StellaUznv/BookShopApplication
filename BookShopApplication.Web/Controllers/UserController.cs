using System.Runtime.CompilerServices;
using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            try
            {

                var userId = this.GetUserId();
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return NotFound();

                var model = new UpdateProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to update your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UpdateProfileViewModel model)
        {
            try
            {

                if (!ModelState.IsValid) return View(model);

                var userId = this.GetUserId();
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return NotFound();

                var result = await _userService.UpdateUserNameAsync(userId, model.FirstName, model.LastName);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = "An Error occured while trying to update your data.";
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }
    }
}
