using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class SaveOrderItemRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? LastOrderItemStatusId { get; set; }
        public Guid? NewOrderItemStatusId { get; set; }
        public string ExpectedDateOfProductOnStock { get; set; }
        public IEnumerable<AttributeValueRequestModel> AttributesValues { get; set; }
    }
}
