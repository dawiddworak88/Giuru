using System.Collections.Generic;

namespace Buyer.Web.Areas.Dashboard.ApiResponseModel
{
    public class ChartResponseModel
    {
        public IEnumerable<string> ChartLabels { get; set; }
        public IEnumerable<ChartDatasetsResponseModel> ChartDatasets { get; set; }
    }
}
