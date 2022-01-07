using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.PageContent.ComponentModels;

namespace Buyer.Web.Shared.ModelBuilders.Catalogs
{
    public interface ICatalogModelBuilder<in S, out T> where S : ComponentModelBase where T : CatalogViewModel, new()
    {
        T BuildModel(S componentModel);
    }
}
