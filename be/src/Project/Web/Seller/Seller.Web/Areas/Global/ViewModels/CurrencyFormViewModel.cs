using System;

namespace Seller.Web.Areas.Global.ViewModels
{
    public class CurrencyFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string CurrenciesUrl { get; set; }
        public string NavigateToCurrencies { get; set; }
        public string IdLabel { get; set; }
        public string CurrencyCodeLabel { get; set; }
        public string CurrencyCode { get; set; }
        public string SymbolLabel { get; set; }
        public string Symbol { get; set; }
        public string NameLabel { get; set; }
        public string Name { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
    }
}
