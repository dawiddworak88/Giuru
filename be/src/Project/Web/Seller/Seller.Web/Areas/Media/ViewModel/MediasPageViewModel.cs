using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Media.ViewModel
{
    public class MediasPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<MediaItem> Catalog { get; set; }
    }
}
