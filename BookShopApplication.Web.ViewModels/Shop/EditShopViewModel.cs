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
    public class EditShopViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = NameRequiredMessage)]
        [StringLength(NameMaxLength, ErrorMessage = NameLengthMessage, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [StringLength(DescriptionMaxLength, ErrorMessage = DescriptionLengthMessage, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;
        [Required]
        public Guid LocationId { get; set; }
    }
}
