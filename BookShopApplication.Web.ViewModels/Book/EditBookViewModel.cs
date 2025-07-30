using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Book
{
    public class EditBookViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int PagesNumber { get; set; }

        public IFormFile? ImageFile { get; set; }
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
