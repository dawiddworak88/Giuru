using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Shared.Catalogs.ModelBuilders
{
    public interface ICatalogModelBuilder
    {
        T BuildModel<T>() where T : CatalogBaseViewModel, new();
    }
}
