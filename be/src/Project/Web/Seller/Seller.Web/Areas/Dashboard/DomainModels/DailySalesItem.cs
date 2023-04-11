namespace Seller.Web.Areas.Dashboard.DomainModels
{
    public class DailySalesItem
    {
        public int DayOfWeek { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Quantity { get; set; }
    }
}
