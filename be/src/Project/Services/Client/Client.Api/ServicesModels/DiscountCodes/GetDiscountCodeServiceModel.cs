using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class GetDiscountCodeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
