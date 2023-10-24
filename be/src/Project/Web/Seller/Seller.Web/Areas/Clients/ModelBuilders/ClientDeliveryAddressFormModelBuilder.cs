using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Clients.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientDeliveryAddressFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientDeliveryAddressFormViewModel>
    {
        public ClientDeliveryAddressFormModelBuilder()
        {

        }

        public async Task<ClientDeliveryAddressFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientDeliveryAddressFormViewModel
            {

            };

            return viewModel;
        }
    }
}
