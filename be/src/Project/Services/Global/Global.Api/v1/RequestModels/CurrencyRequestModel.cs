using Foundation.ApiExtensions.Models.Request;

namespace Global.Api.v1.RequestModels
{
    public class CurrencyRequestModel : RequestModelBase
    {
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
