using BookShopApplication.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookShopApplication.Web.ViewModels.Book
{
    public class CreateBookViewModel
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

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
        public Guid? GenreId { get; set; }  // Nullable so user can choose new genre instead

        [Required]
        public Guid ShopId { get; set; }

        // List of genres for the dropdown
        public IEnumerable<SelectListItem>? Genres { get; set; }
    }

}
