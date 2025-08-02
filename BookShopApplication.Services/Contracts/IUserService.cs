using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopApplication.Services.Contracts
{
    public interface IUserService
    {
        public Task<IEnumerable<SelectListItem>> GetUsersAsync();
    }
}
