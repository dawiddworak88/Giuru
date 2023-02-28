using System;
using System.Collections.Generic;

namespace Seller.Web.Shared.ViewModels
{
    public class ChartViewModel
    {
        public string Title { get; set; }
        public string FromLabel { get; set; }
        public string ToLabel { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public IEnumerable<string> ChartLabels { get; set; }
        public IEnumerable<ChartDatasetsViewModel> ChartDatasets { get; set; }
    }
}
