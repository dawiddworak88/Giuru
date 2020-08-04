using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Shared.Catalogs.ModelBuilders
{
    public interface ICatalogModelBuilder
    {
        T BuildModel<T>() where T : CatalogBaseViewModel, new();
    }
}
