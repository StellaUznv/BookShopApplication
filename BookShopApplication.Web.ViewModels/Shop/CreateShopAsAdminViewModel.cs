using BookShopApplication.Web.ViewModels.Location;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Shop
{
    public class CreateShopAsAdminViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public CreateLocationViewModel Location { get; set; } = new();

        [Required]
        public Guid SelectedManagerId { get; set; }

        public IEnumerable<SelectListItem> Managers { get; set; } = new List<SelectListItem>();
    }
}
