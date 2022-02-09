using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class DeleteOutletServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
