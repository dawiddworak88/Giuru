using Foundation.ApiExtensions.Models.Request;
using System;

namespace Foundation.Pricing.ApiRequestModels
{
    /// <summary>The query of the Client.Api validation endpoint. Its properties become the query string.</summary>
    public class DiscountCodeValidationApiRequestModel : RequestModelBase
    {
        public string Code { get; set; }
        public Guid? ClientId { get; set; }
    }
}
