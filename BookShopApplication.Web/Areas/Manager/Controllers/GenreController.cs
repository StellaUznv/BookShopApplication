﻿using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Areas.Manager.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "Invalid genre data." });
            }

            var success = await _genreService.AddNewGenreAsync(model);

            if (!success)
            {
                return Json(new { success = false, error = "Genre already exists." });
            }

            return Json(new { success = true, id = model.Id, name = model.Name });
        }
    }
}
