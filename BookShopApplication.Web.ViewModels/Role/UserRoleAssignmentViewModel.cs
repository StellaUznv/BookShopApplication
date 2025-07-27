using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Role
{
    public class UserRoleAssignmentViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public bool IsAssigned { get; set; }
    }
}
