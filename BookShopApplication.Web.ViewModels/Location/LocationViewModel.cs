using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Location
{
    public class LocationViewModel
    {
        [Required] 
        public Guid Id { get; set; }

        [Required]
        public string CountryName { get; set; } = null!;

        [Required]
        public string CityName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string ZipCode { get; set; } = null!;
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

    }
}
