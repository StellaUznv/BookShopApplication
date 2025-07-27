using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Role
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(256)]
        public string Name { get; set; } = null!;
    }
}
