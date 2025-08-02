using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<SelectListItem>> GetUsersAsync()
        {
            var users = await _userManager.Users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Email
            }).ToListAsync();

            return users;
        }
    }
}
