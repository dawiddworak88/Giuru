﻿using System;

namespace Seller.Web.Areas.Global.DomainModels
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
