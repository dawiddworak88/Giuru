using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Schemas.Models
{
    public class OrderValidationModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
    }
}
