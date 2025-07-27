using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalShops { get; set; }
        public int TotalRoles { get; set; }
        public int TotalLocations { get; set; }
        public int TotalGenres { get; set; }
        public int TotalUsers { get; set; }
    }

}
