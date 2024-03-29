﻿using System;

namespace Seller.Web.Areas.Global.DomainModels
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set;}
    }
}
