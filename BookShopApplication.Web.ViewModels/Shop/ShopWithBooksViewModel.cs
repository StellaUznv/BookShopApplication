﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Web.ViewModels.Book;

namespace BookShopApplication.Web.ViewModels.Shop
{
    public class ShopWithBooksViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string LocationAddress { get; set; } = null!;
        public string LocationCity { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<BookViewModel> BooksInShop { get; set; } = new List<BookViewModel>();
    }
}
