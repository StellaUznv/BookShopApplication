﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Web.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid BookId { get; set; }

        public string Title { get; set; } = null!;

        public string? ImagePath { get; set; }

        public string Price { get; set; } = null!;

        public int Quantity { get; set; }

    }
}
