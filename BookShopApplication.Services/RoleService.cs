using BookShopApplication.Data.Models;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                .ToListAsync();

            return roles;
        }

        public async Task<bool> RoleExistsAsync(CreateRoleViewModel role)
        {
            return  await _roleManager.RoleExistsAsync(role.Name);
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel role)
        {
            return await _roleManager.CreateAsync(new ApplicationRole(role.Name));
        }

        public async Task<RoleEditViewModel> GetRoleToEditAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new ArgumentNullException(role.Name, "The role you're looking for is not found.");
            }

            var model = new RoleEditViewModel
            {
                Id = role.Id,
                Name = role.Name!
            };

            foreach (var user in _userManager.Users.ToList())
            {
                model.Users.Add(new UserRoleAssignmentViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    IsAssigned = await _userManager.IsInRoleAsync(user, role.Name!)
                });
            }

            return model;
        }

        public async Task<IdentityResult> EditRoleAsync(RoleEditViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
            {
                throw new ArgumentNullException(role.Name, "The role you're looking for is not found.");
            }
            role.Name = model.Name;
            return  await _roleManager.UpdateAsync(role);
        }

        public async Task<bool> AssignRoleAsync(string currentUserId,RoleEditViewModel model)
        {
            

            foreach (var userModel in model.Users)
            {
                var user = await _userManager.FindByIdAsync(userModel.UserId.ToString());
                if (user == null) continue;

                var isInRole = await _userManager.IsInRoleAsync(user, model.Name!);

                if (userModel.IsAssigned && !isInRole)
                    await _userManager.AddToRoleAsync(user, model.Name!);
                else if (!userModel.IsAssigned && isInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, model.Name!);
                    if (user.Id.ToString() == currentUserId)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        return true;
                    }
                }

            }

            return false;
        }

        public async Task<IdentityResult> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new ArgumentNullException(role.Name, "The role you're looking for is not found.");
            }

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<bool> AssignManagerRoleAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Manager"))
            {
                await _userManager.AddToRoleAsync(user, "Manager");

                await _signInManager.RefreshSignInAsync(user);
                return true;
            }

            return false;
        }
    }
}
