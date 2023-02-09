using System.Collections.Generic;

namespace Seller.Web.Shared.ViewModels
{
    public class ChartViewModel
    {
        public string Title { get; set; }
        public IEnumerable<string> ChartLables { get; set; }
        public IEnumerable<ChartDatasetsViewModel> ChartDatasets { get; set; }
    }
}
