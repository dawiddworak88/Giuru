using Foundation.Extensions.Models;
using System;

namespace Global.Api.ServicesModels.Currencies
{
    public class UpdateCurrencyServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
