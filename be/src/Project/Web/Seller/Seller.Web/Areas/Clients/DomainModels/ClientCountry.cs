﻿using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ClientCountry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}