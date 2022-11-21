using System;

namespace Seller.Web.Areas.Global.ViewModels
{
    public class CountryFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string CountriesUrl { get; set; }
        public string NavigateToCountries { get; set; }
        public string IdLabel { get; set; }
        public string NameLabel { get; set; }
        public string Name { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
    }
}
