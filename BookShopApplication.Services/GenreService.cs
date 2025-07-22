using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Genre;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreViewModel>> GetGenreListAsync()
        {
            var genres = await _genreRepository.GetAllAttached().Select(g => new GenreViewModel
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
            }).ToListAsync();

            return genres;
        }
    }
}
