using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class GetOrderItemStatusesHistoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
