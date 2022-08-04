using Buyer.Web.Shared.ViewModels.Files;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterCategoryDetailsViewModel
    {
        public string Title { get; set; }
        public string NoCategoriesLabel { get; set; }
        public IEnumerable<DownloadCenterCategoryDetailViewModel> Subcategories { get; set; }
        public FilesViewModel Files { get; set; }
    }
}
