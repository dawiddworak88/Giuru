﻿using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderAttributeViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public IEnumerable<OrderAttributeOptionViewModel> Options { get; set; }
    }
}