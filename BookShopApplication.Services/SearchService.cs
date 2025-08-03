using BookShopApplication.Data;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Search;
using BookShopApplication.Web.ViewModels.Search.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Repository.Contracts;

namespace BookShopApplication.Services
{
    public class SearchService : ISearchService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IShopRepository _shopRepository;

        public SearchService( IShopRepository shopRepository,IBookRepository bookRepository)
        {
            _shopRepository = shopRepository;
            _bookRepository = bookRepository;
        }

        public async Task<SearchResultDto> SearchAsync(string query, string filter, int page, int pageSize)
        {
            query = query.ToLower().Trim();
            filter = filter.ToLower().Trim();

            var results = new List<SearchResultItemDto>();

            if (filter == "all" || filter == "book")
            {
                var books = await _bookRepository.GetAllAttached()
                    .Where(b => b.Title.ToLower().Contains(query) || b.AuthorName.ToLower().Contains(query))
                    .Select(b => new SearchResultItemDto
                    {
                        Type = "Book",
                        Id = b.Id,
                        Title = b.Title,
                        Subtitle = b.AuthorName,
                        Url = $"/Book/Details/{b.Id}"
                    })
                    .ToListAsync();

                results.AddRange(books);
            }

            if (filter == "all" || filter == "shop")
            {
                var shops = await _shopRepository.GetAllAttached()
                    .Where(s => s.Name.ToLower().Contains(query) || s.Description.ToLower().Contains(query))
                    .Select(s => new SearchResultItemDto
                    {
                        Type = "Shop",
                        Id = s.Id,
                        Title = s.Name,
                        Subtitle = s.Description,
                        Url = $"/Shop/Details/{s.Id}"
                    })
                    .ToListAsync();

                results.AddRange(shops);
            }

            if (filter == "location")
            {
                var locations = await _shopRepository.GetAllAttached()
                    .Where(s => s.Location.CityName.ToLower().Contains(query) || s.Location.CountryName.ToLower().Contains(query) 
                                                                              || s.Location.Address.ToLower().Contains(query))
                    .Select(s => new SearchResultItemDto
                    {
                        Type = "Shop",
                        Id = s.Id,
                        Title = s.Name,
                        Subtitle = s.Location.Address,
                        Url = $"/Shop/Details/{s.Id}"
                    })
                    .ToListAsync();

                results.AddRange(locations);
            }

            if ( filter == "genre")
            {
                var genres = await _bookRepository.GetAllAttached()
                    .Where(b => b.Genre.Name.ToLower().Contains(query))
                    .Select(b => new SearchResultItemDto
                    {
                        Type = "Book",
                        Id = b.Id,
                        Title = b.Title,
                        Subtitle = b.Genre.Name,
                        Url = $"/Book/Details/{b.Id}"
                    })
                    .ToListAsync();

                results.AddRange(genres);
            }

            var totalCount = results.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedResults = results
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new SearchResultDto
            {
                TotalResults = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                Results = pagedResults
            };
        }


    }

}
