using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public NewsCatalogViewModel NewsCatalog { get; set; }
    }
}
