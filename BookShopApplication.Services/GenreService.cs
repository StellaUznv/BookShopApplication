using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Genre;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IBookRepository _bookRepository;

        public GenreService(IGenreRepository genreRepository, IBookRepository bookRepository)
        {
            _genreRepository = genreRepository;
            _bookRepository = bookRepository;
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

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            var genres = await _genreRepository.GetAllAsync();
            return genres;
        }

        public async Task<bool> AddNewGenreAsync(CreateGenreViewModel model)
        {
            var genres = await GetAllGenresAsync();

            if (genres.Select(g=>g.Name.ToLower()).Contains(model.Name.ToLower()))
            {
                if (genres.First(g=>g.Name == model.Name).IsDeleted)
                {
                    var genre = genres.First(g => g.Name == model.Name);
                    genre.IsDeleted = false;
                    return await _genreRepository.UpdateAsync(genre);
                }
                else
                {
                    return false;
                }
            }
            
            var genreToAdd = new Genre
            {
                Description = model.Description,
                Id = model.Id,
                Name = model.Name,
            };
            return await _genreRepository.AddAsync(genreToAdd);
            
        }

        public async Task<EditGenreViewModel> GetGenreToEditAsync(Guid genreId)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);

            var model = new EditGenreViewModel
            {
                Id = genreId,
                Description = genre.Description,
                Name = genre.Name
            };

            return model;
        }

        public async Task<bool> EditGenreAsync(EditGenreViewModel model)
        {
            var genre = new Genre
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
            };
           return  await _genreRepository.UpdateAsync(genre);
        }

        public async Task<bool> DeleteGenreAsync(Guid genreId)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);

            if (await _bookRepository.AnyAsync(b=>b.GenreId == genreId))
            {
                return false;
            }

            return await _genreRepository.SoftDeleteAsync(genre);
        }
    }
}
