﻿using Foundation.PageContent.ComponentModels;
using System;

namespace Buyer.Web.Areas.Products.ComponentModels
{
    public class PriceComponentModel : ComponentModelBase
    {
        public Guid? ClientId { get; set; }
        public string CurrencyCode { get; set; }
        public string ExtraPacking { get; set; }
        public string PaletteLoading { get; set; }
        public string OwnTransport {  get; set; }
        public string Country { get; set; }
        public string DeliveryZipCode { get; set; }
    }
}
