using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }

        [Required] 
        public string CountryName { get; set; } = null!;
        [Required]
        public string CityName { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
        [Required]
        public ICollection<Shop> Shops { get; set; } = new List<Shop>();
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
