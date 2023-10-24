using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientDeliveryAddressesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientDeliveryAddress>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;

        public ClientDeliveryAddressesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder)
        {
            _catalogModelBuilder = catalogModelBuilder;
        }

        public async Task<CatalogViewModel<ClientDeliveryAddress>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<ClientDeliveryAddress>, ClientDeliveryAddress>();

            return viewModel;
        }
    }
}
