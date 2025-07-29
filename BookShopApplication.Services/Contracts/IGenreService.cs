using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Web.ViewModels.Genre;

namespace BookShopApplication.Services.Contracts
{
    public interface IGenreService
    {
        public Task<IEnumerable<GenreViewModel>> GetGenreListAsync();

        public Task<IEnumerable<Genre>> GetAllGenresAsync();

        public Task<bool> AddNewGenreAsync(CreateGenreViewModel model);

        public Task<EditGenreViewModel> GetGenreToEditAsync(Guid genreId);
        public Task<bool> EditGenreAsync(EditGenreViewModel model);
    }
}
