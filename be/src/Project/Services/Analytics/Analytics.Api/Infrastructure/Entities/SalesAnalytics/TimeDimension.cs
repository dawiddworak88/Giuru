using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class TimeDimension : Entity
    {
        [Required]
        public int Hour { get; set; }

        [Required]
        public int Minute { get; set; }

        [Required]
        public int Second { get; set; }

        [Required]
        public int DayOfWeek { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Quarter { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
