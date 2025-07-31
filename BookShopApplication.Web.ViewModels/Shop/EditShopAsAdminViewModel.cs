using BookShopApplication.Web.ViewModels.Location;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.ShopConstraints;
using static BookShopApplication.GCommon.ValidationErrorMessages.ModelErrorMessages.ShopMessages;

namespace BookShopApplication.Web.ViewModels.Shop
{
    public class EditShopAsAdminViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = NameRequiredMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        public string Description { get; set; } = null!;

        [Required]
        public EditLocationViewModel Location { get; set; } = null!;

        [Required]
        public Guid SelectedManagerId { get; set; }
        [Required]
        public IEnumerable<SelectListItem> Managers { get; set; } = new List<SelectListItem>();
    }
}

