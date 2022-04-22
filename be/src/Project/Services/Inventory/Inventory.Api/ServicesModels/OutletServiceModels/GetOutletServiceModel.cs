using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class GetOutletServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
