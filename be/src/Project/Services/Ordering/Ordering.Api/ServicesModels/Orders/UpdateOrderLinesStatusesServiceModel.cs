using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.Orders
{
    public class UpdateOrderLinesStatusesServiceModel : BaseServiceModel
    {
        public IEnumerable<UpdateOrderLinesStatusServiceModel> OrderItems { get; set; }
    }
}
