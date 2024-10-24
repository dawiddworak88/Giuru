﻿using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderItemStatusServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderItemStatusId { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
    }
}
