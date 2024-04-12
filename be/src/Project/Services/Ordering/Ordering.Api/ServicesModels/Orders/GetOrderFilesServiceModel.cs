using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels.Orders
{
    public class GetOrderFilesServiceModel : PagedBaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
