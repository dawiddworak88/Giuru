using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class DeleteOutletServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
