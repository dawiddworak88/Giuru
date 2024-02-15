using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Global.ApiRequestModels
{
    public class CurrencyRequestModel : RequestModelBase
    {
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
