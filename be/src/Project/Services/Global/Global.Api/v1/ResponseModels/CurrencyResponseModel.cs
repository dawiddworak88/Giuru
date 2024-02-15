using Foundation.ApiExtensions.Models.Response;
using System;

namespace Global.Api.v1.ResponseModels
{
    public class CurrencyResponseModel
    {
        public Guid? Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
