using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class UpdateOrderItemsStatusesServiceModel : BaseServiceModel
    {
        public IEnumerable<UpdateOrderItemsStatusServiceModel> OrderItems { get; set; }
    }
}
