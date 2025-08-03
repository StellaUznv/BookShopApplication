using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Search;
using BookShopApplication.Web.ViewModels.Search.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Services.Contracts
{
    public interface ISearchService
    {
        public Task<SearchResultDto> SearchAsync(string query, string filter, int page, int pageSize);
    }

}
