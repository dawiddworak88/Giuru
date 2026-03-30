using Foundation.Localization;
using Microsoft.Extensions.Localization;

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

        public string GetDeliveryMessage(string deliveryType, bool onStock)
        {
            if (deliveryType == OwnTransportDeliveryType)
            {
                return onStock
                    ? _productLocalizer["OwnTransportInStockMessage"]
                    : _productLocalizer["StandardLeadTimeMessage"];
            }

            return _productLocalizer["StandardDeliveryLeadTimeMessage"];
        }
    }
}
