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
        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> UpdateUserNameAsync(string userId, string firstName, string lastName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            user.FirstName = firstName;
            user.LastName = lastName;
            return await _userManager.UpdateAsync(user);
        }
        public async Task<string?> GetFullNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            return $"{user.FirstName} {user.LastName}";
        }


    }
}
