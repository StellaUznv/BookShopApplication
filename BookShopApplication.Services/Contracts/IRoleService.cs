using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Role;
using Microsoft.AspNetCore.Identity;

namespace BookShopApplication.Services.Contracts
{
    public interface IRoleService
    {
        public Task<IEnumerable<RoleViewModel>> GetAllRolesAsync();

        public Task<bool> RoleExistsAsync(CreateRoleViewModel role);

        public Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel role);

        public Task<RoleEditViewModel> GetRoleToEditAsync(Guid id);

        public Task<IdentityResult> EditRoleAsync(RoleEditViewModel model);

        public Task<bool> AssignRoleAsync(string currentUserId,RoleEditViewModel model);

        public Task<IdentityResult> DeleteRoleAsync(string id);
        public Task<bool> AssignManagerRoleAsync(string id);
    }
}
