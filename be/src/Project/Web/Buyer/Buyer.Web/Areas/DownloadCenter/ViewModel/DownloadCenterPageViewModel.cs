using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public DownloadCenterCatalogViewModel Catalog { get; set; }
    }
}
