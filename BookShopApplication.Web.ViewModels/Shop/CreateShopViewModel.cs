﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Shop
{
    public class CreateShopViewModel
    {

        
        [Required] 
        public string Name { get; set; } = null!;

        [Required] 
        public string Description { get; set; } = null!;

       

    }
}
