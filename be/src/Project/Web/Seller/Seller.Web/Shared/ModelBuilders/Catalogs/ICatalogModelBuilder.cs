using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Shared.Catalogs.ModelBuilders
{
    public interface ICatalogModelBuilder
    {
        T BuildModel<T, W>() where T : CatalogViewModel<W>, new() where W : class;
    }
}