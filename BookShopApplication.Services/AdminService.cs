using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AdminService(IBookRepository bookRepository, IShopRepository shopRepository, IGenreRepository genreRepository, ILocationRepository locationRepository
        ,UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._bookRepository = bookRepository;
            this._shopRepository = shopRepository;
            this._genreRepository = genreRepository;
            this._locationRepository = locationRepository;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<AdminDashboardViewModel> GetAdminDashboardData()
        {
            var model = new AdminDashboardViewModel
            {
                TotalBooks = await _bookRepository.GetCountAsync(),
                TotalGenres = await _genreRepository.GetCountAsync(),
                TotalLocations = await _locationRepository.GetCountAsync(),
                TotalShops = await _shopRepository.GetCountAsync(),
                TotalRoles = await _roleManager.Roles.CountAsync(),
                TotalUsers = await _userManager.Users.CountAsync()

            };
            return model;
        }
    }
}
