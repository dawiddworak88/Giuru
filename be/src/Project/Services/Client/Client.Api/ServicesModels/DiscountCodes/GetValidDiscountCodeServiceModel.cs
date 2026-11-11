using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class GetValidDiscountCodeServiceModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }

        public string Code { get; set; }
    }
}
