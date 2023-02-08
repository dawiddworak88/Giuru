using System;
using System.Collections.Generic;

namespace Analytics.Api.v1.RequestModels
{
    public class SalesAnalyticsItemRequestModel
    {
        public Guid? CountryId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SalesAnalyticsItemProduct> Products { get; set; }
        public IEnumerable<SalesAnalyticsItemCountryTranslationRequestModel> CountryTranslations { get; set; }
    }
}
