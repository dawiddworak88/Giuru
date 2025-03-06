﻿using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class Approval
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
