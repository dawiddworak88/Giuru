using System;
using System.Collections.Generic;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class CreateSalesAnalyticsItemServiceModel
    {
        public Guid? ClientId { get; set; }
        public Guid? CountryId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<CreateSalesAnalyticsProductServiceModel> Products { get; set; }
        public IEnumerable<CreateSalesAnalyticsCountryServiceModel> CountryTranslations { get; set; }
    }
}
