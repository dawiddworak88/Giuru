﻿using System;
using System.Collections.Generic;
using Ordering.Api.ServicesModels.OrderAttributeOptions;

namespace Ordering.Api.ServicesModels.OrderAttributes
{
    public class OrderAttributeServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOrderItemAttribute { get; set; }
        public IEnumerable<AttributeOptionServiceModel> OrderAttributeOptions { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}