using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models.Contracts;

namespace BookShopApplication.Data.Models
{
    public class Location : ISoftDeletable
    {
        [Key]
        public Guid Id { get; set; }

        [Comment("Name of the Country in Location entity")]
        public string CountryName { get; set; } = null!;
        [Comment("Name of the City in Location entity")]
        public string CityName { get; set; } = null!;
        [Comment("Address line")]
        public string Address { get; set; } = null!;
        [Comment("PostalCode in Location entity")]
        public string ZipCode { get; set; } = null!;
        [Comment("Reference collection to Shop entity")]
        public ICollection<Shop> Shops { get; set; } = new List<Shop>();
        [Comment("Tells if the Location is Soft Deleted or not")]
        public bool IsDeleted { get; set; } = false;
    }
}
