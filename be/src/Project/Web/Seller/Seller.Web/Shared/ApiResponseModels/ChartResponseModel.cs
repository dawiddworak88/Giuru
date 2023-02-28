using System.Collections.Generic;

namespace Seller.Web.Shared.ApiResponseModels
{
    public class ChartResponseModel
    {
        public IEnumerable<string> ChartLabels { get; set; }
        public IEnumerable<ChartDatasetsResponseModel> ChartDatasets { get; set; }
    }
}
