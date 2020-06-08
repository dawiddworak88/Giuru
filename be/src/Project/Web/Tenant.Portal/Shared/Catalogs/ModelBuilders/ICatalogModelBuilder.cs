using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Shared.Catalogs.ModelBuilders
{
    public interface ICatalogModelBuilder
    {
        T BuildModel<T>() where T : CatalogBaseViewModel, new();
    }
}
