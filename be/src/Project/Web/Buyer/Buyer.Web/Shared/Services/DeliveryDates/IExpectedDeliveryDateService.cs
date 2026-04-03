using System;

namespace Buyer.Web.Shared.Services.DeliveryDates
{
    public interface IExpectedDeliveryDateService
    {
        DateOnly CalculateExpectedDeliveryDate(int deliveryBusinessDays);
        DateOnly CalculateExpectedDeliveryDate(int deliveryBusinessDays, DateTime now);
    }
}
