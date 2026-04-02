using System;

namespace Buyer.Web.Shared.Services.DeliveryDates
{
    public interface IExpectedDeliveryDateService
    {
        DateTime CalculateExpectedDeliveryDate(int deliveryBusinessDays);
        DateTime CalculateExpectedDeliveryDate(int deliveryBusinessDays, DateTime now);
    }
}
