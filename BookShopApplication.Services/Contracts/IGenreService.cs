using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Genre;

namespace BookShopApplication.Services.Contracts
{
    public interface IGenreService
    {
        public Task<IEnumerable<GenreViewModel>> GetGenreListAsync();
    }
}
