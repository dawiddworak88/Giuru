using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.Services.DeliveryDates
{
    public class ExpectedDeliveryDateService : IExpectedDeliveryDateService
    {
        private const int ShipmentOnTheSameDayByHour = 14;
        private static readonly TimeZoneInfo PolandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        private static readonly HashSet<string> PermanentHolidays = new()
        {
            "01-01", "01-06", "05-01", "05-03",
            "08-15", "11-01", "11-11",
            "12-24", "12-25", "12-26"
        };

        private static readonly HashSet<string> MovableHolidays = new()
        {
            "2026-04-05", "2026-04-06", "2026-05-24", "2026-06-04",
            "2027-03-28", "2027-03-29", "2027-05-16", "2027-05-27",
            "2028-04-16", "2028-04-17", "2028-06-04", "2028-06-15",
            "2029-04-01", "2029-04-02", "2029-05-20", "2029-05-31",
            "2030-04-21", "2030-04-22", "2030-06-09", "2030-06-20",
            "2031-04-13", "2031-04-14", "2031-06-01", "2031-06-12",
            "2032-03-28", "2032-03-29", "2032-05-16", "2032-05-27",
            "2033-04-17", "2033-04-18", "2033-06-05", "2033-06-16"
        };

        public DateOnly CalculateExpectedDeliveryDate(int deliveryBusinessDays)
        {
            var nowInPoland = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, PolandTimeZone);
            return CalculateExpectedDeliveryDate(deliveryBusinessDays, nowInPoland);
        }

        public DateOnly CalculateExpectedDeliveryDate(int deliveryBusinessDays, DateTime now)
        {
            var currentDate = GetShipmentStartDate(now);

            var addedBusinessDays = 0;

            while (addedBusinessDays < deliveryBusinessDays)
            {
                currentDate = currentDate.AddDays(1);

                if (IsBusinessDay(currentDate))
                {
                    addedBusinessDays++;
                }
            }

            return DateOnly.FromDateTime(currentDate);
        }

        private static DateTime GetShipmentStartDate(DateTime now)
        {
            if (now.Hour < ShipmentOnTheSameDayByHour)
            {
                return now.AddDays(-1);
            }

            return now;
        }

        private static bool IsBusinessDay(DateTime date)
        {
            if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                return false;
            }

            if (PermanentHolidays.Contains(date.ToString("MM-dd")))
            {
                return false;
            }

            if (MovableHolidays.Contains(date.ToString("yyyy-MM-dd")))
            {
                return false;
            }

            return true;
        }
    }
}
