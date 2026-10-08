using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class ValidateDiscountCodeServiceModel : BaseServiceModel
    {
        public string Code { get; set; }
        public Guid? ClientId { get; set; }

        /// <summary>Taken from the authenticated principal, never from the request.</summary>
        public bool IsSeller { get; set; }
    }
}
