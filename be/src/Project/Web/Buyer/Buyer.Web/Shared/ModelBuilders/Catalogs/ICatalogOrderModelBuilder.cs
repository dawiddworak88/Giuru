using Buyer.Web.Shared.ViewModels.Catalogs;

namespace Buyer.Web.Shared.ModelBuilders.Catalogs
{
    public interface ICatalogOrderModelBuilder
    {
        T BuildModel<T, W>() where T : CatalogOrderViewModel<W>, new() where W : class;
    }
}