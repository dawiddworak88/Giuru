﻿using Buyer.Web.Areas.Products.DomainModels;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductSuggestionViewModel
    {
        public string Name { get; set; }
        public string Attributes { get; set; }
        public string Sku { get; set; }
        public string Url { get; set; }
    }
}