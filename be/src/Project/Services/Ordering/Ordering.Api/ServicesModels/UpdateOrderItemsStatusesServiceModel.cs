using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderItemsStatusesServiceModel : BaseServiceModel 
    {
        public IEnumerable<UpdateOrderItemsStatusServiceModel> OrderItems { get; set; }
    }
}
