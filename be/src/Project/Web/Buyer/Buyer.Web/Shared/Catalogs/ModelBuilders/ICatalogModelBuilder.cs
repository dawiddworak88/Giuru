using Buyer.Web.Shared.Catalogs.ViewModels;
using Foundation.PageContent.ComponentModels;

namespace Buyer.Web.Shared.Catalogs.ModelBuilders
{
    public interface ICatalogModelBuilder<in S, out T> where S : ComponentModelBase where T : CatalogViewModel, new()
    {
        T BuildModel(S componentModel);
    }
}
