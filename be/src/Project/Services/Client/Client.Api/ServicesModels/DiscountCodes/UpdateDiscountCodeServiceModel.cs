using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class UpdateDiscountCodeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
    }
}
