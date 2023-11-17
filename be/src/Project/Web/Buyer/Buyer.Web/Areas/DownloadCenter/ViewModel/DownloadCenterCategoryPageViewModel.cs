using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterCategoryPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public DownloadCenterCategoryBreadcrumbsViewModel Breadcrumbs { get; set; }
        public DownloadCenterCategoryDetailsViewModel CategoryDetails { get; set; }
    }
}
