using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class GetOutletServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
