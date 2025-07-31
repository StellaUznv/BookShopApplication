using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.RoleConstraints;
using static BookShopApplication.GCommon.ValidationErrorMessages.ModelErrorMessages.RoleMessages;

namespace BookShopApplication.Web.ViewModels.Role
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        [StringLength(NameMaxLength , ErrorMessage = NameLengthMessage, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
