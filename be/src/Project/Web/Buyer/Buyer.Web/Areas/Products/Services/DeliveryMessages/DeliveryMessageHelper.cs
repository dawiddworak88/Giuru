using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace Buyer.Web.Areas.Products.Services.DeliveryMessages
{
    public class DeliveryMessageHelper : IDeliveryMessageHelper
    {
        private const string OwnTransportDeliveryType = "Own transport";

        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public DeliveryMessageHelper(IStringLocalizer<ProductResources> productLocalizer)
        {
            _productLocalizer = productLocalizer;
        }

        public string GetDeliveryMessage(string deliveryType, bool onStock, DateTime? expectedDelivery = null)
        {
            string message;

            if (deliveryType == OwnTransportDeliveryType)
            {
                message = onStock
                    ? _productLocalizer["OwnTransportInStockMessage"]
                    : _productLocalizer["StandardLeadTimeMessage"];
            }
            else
            {
                message = _productLocalizer["StandardDeliveryLeadTimeMessage"];
            }

            if (expectedDelivery.HasValue)
            {
                var formattedDate = expectedDelivery.Value.ToString("d.MM", CultureInfo.InvariantCulture);
                var suffix = _productLocalizer["ExpectedDeliveryDateSuffix"].Value
                    .Replace("{expectedDelivery}", formattedDate);

                message += " " + suffix;
            }

            return message;
        }
    }
}
