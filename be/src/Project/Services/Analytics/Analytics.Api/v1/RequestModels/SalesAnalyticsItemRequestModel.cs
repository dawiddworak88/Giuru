﻿using System;
using System.Collections.Generic;

namespace Analytics.Api.v1.RequestModels
{
    public class SalesAnalyticsItemRequestModel
    {
        public Guid? CountryId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? ProductId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string Ean { get; set; }
        public bool IsStock { get; set; }
        public bool IsOutlet { get; set; }
        public double Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SalesAnalyticsItemCountryTranslationRequestModel> CountryTranslations { get; set; }
    }
}