using Foundation.Extensions.Models;

namespace Global.Api.ServicesModels.Currencies
{
    public class CreateCurrencyServiceModel : BaseServiceModel
    {
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
