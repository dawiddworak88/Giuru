using Buyer.Web.Shared.Services.DeliveryDates;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace Buyer.Web.Areas.Products.Services.DeliveryMessages
{
    public class DeliveryMessageHelper : IDeliveryMessageHelper
    {
        private const string OwnTransportDeliveryType = "Own transport";
        private static readonly TimeZoneInfo PolandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IExpectedDeliveryDateService _expectedDeliveryDateService;

        public DeliveryMessageHelper(
            IStringLocalizer<ProductResources> productLocalizer,
            IExpectedDeliveryDateService expectedDeliveryDateService)
        {
            _productLocalizer = productLocalizer;
            _expectedDeliveryDateService = expectedDeliveryDateService;
        }

        public string GetDeliveryMessage(string deliveryType, bool onStock, int leadTimeDays, DateTime? expectedDelivery = null)
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

            if (leadTimeDays > 0)
            {
                var deliveryDate = _expectedDeliveryDateService.CalculateExpectedDeliveryDate(leadTimeDays);
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, PolandTimeZone).Date;
                var days = (deliveryDate - now).Days;
                var date = deliveryDate.ToString("d.MM", CultureInfo.InvariantCulture);

                message = message
                    .Replace("{date}", date)
                    .Replace("{days}", days.ToString());
            }

            return message;
        }
    }
}
