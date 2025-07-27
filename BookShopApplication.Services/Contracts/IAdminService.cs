using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Web.ViewModels.Admin;

namespace BookShopApplication.Services.Contracts
{
    public interface IAdminService
    {
        public Task<AdminDashboardViewModel> GetAdminDashboardData();
    }
}
