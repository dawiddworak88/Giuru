using System;

namespace Buyer.Web.Areas.Products.Services.DeliveryMessages
{
    public interface IDeliveryMessageHelper
    {
        string GetDeliveryMessage(string deliveryType, bool onStock, DateTime? expectedDelivery = null);
    }
}
