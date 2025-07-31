using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookShopApplication.GCommon.CustomValidationAttributes.CustomValidationAttributes;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.BookConstraints;
using static BookShopApplication.GCommon.ValidationErrorMessages.ModelErrorMessages.BookMessages;

namespace BookShopApplication.Web.ViewModels.Book
{
    public class EditBookViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = TitleRequiredMessage)]
        [StringLength(TitleMaxLength,ErrorMessage = TitleLengthMessage,MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [StringLength(DescriptionMaxLength, ErrorMessage = DescriptionLengthMessage, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = AuthorRequiredMessage)]
        [StringLength(AuthorNameMaxLength, ErrorMessage = AuthorLengthMessage, MinimumLength = AuthorNameMinLength)]
        public string Author { get; set; } = null!;

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(PriceMinValue,PriceMaxValue,ErrorMessage = PriceNotInRangeMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = PagesRequiredMessage)]
        [Range(PagesMinValue,PagesMaxValue,ErrorMessage = PagesNotInRangeMessage)]
        public int PagesNumber { get; set; }

        [MaxFileSize(FileMaxSize)]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" }, ErrorMessage = FileExtensionErrorMessage)]
        public IFormFile? ImageFile { get; set; }

        [StringLength(ImagePathMaxLength)]
        public string? ImagePath { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required(ErrorMessage = "Please select a genre or enter a new one.")]
        public Guid GenreId { get; set; }  

        [Required]
        public Guid ShopId { get; set; }

        // List of genres for the dropdown
        public IEnumerable<SelectListItem>? Genres { get; set; }
    }
}
