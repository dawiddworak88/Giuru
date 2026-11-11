using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class DeleteDiscountCodeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
