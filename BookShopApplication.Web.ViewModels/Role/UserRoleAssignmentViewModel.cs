using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Role
{
    public class UserRoleAssignmentViewModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public bool IsAssigned { get; set; }
    }
}
