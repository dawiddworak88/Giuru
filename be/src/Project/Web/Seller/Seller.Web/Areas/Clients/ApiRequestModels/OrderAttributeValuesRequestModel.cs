using System.Collections.Generic;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class OrderAttributeValuesRequestModel
    {
        public Guid? OrderId { get; set; }
        public IEnumerable<OrderAttributeValueRequestModel> Values { get; set; }
    }
}
