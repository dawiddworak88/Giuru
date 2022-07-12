using Buyer.Web.Areas.DownloadCenter.ModelBuilders;
using Buyer.Web.Shared.ViewModels.Files;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class CategoryDetailsViewModel
    {
        public string Title { get; set; }
        public IEnumerable<CategoryDetail> Categories { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
