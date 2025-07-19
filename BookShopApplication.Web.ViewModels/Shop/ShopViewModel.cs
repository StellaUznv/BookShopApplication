using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Shop
{
    public class ShopViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string LocationAddress { get; set; } = null!;
        public string LocationCity { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
