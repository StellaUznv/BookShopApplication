using BookShopApplication.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Services.Contracts
{
    public interface IUserService
    {
        public Task<IEnumerable<SelectListItem>> GetUsersAsync();

        public Task<ApplicationUser?> GetUserByIdAsync(string userId);
        public Task<IdentityResult> UpdateUserNameAsync(string userId, string firstName, string lastName);
        public Task<string?> GetFullNameAsync(string userId);
    }
}
