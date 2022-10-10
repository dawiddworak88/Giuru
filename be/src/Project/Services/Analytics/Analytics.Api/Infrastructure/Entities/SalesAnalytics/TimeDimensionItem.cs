using Foundation.GenericRepository.Entities;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class TimeDimensionItem : Entity
    {
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int DayOfWeek { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
    }
}
