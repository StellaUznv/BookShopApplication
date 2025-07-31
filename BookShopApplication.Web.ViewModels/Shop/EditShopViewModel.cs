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
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        public string Description { get; set; } = null!;
        [Required]
        public Guid LocationId { get; set; }
    }
}
