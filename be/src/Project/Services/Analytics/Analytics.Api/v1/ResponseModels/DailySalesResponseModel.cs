namespace Analytics.Api.v1.ResponseModels
{
    public class DailySalesResponseModel
    {
        public int Day { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Quantity { get; set; }
    }
}
