using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookShopApplication.GCommon.ValidationConstraints;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints;
using static BookShopApplication.GCommon.ValidationErrorMessages.ModelErrorMessages;

namespace BookShopApplication.Data.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = BookMessages.TitleRequired)]
        [StringLength(BookConstraints.TitleMaxLength, ErrorMessage = BookMessages.TitleMaxLength)]
        [MinLength(BookConstraints.TitleMinLength, ErrorMessage = BookMessages.TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = BookMessages.DescriptionRequired)]
        [StringLength(BookConstraints.DescriptionMaxLength,ErrorMessage = BookMessages.DescriptionMaxLength)]
        [MinLength(BookConstraints.DescriptionMinLength, ErrorMessage = BookMessages.DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = BookMessages.AuthorRequired)]
        [StringLength(BookConstraints.AuthorNameMaxLength, ErrorMessage = BookMessages.AuthorMaxLength)]
        [MinLength(BookConstraints.AuthorNameMinLength, ErrorMessage = BookMessages.AuthorMinLength)]
        public string AuthorName { get; set; } = null!;
        [Required(ErrorMessage = BookMessages.PriceRequired)]
        [Range(BookConstraints.PriceMinValue
            ,BookConstraints.PriceMaxValue,ErrorMessage = BookMessages.PriceNotInRange)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = BookMessages.PagesRequired)]
        [Range(BookConstraints.PagesMinValue
            ,BookConstraints.PagesMaxValue , ErrorMessage = BookMessages.PagesNotInRange)]
        public int PagesNumber { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        
        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        [Required]
        public Guid GenreId { get; set; } 
        
        [Required]
        public ICollection<BookInShop> BookInShops { get; set; } = new List<BookInShop>();

    }
}
