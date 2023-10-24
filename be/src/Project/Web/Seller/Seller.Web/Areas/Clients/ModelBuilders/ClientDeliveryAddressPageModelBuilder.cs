using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Clients.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientDeliveryAddressPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientDeliveryAddressPageViewModel>
    {
        public ClientDeliveryAddressPageModelBuilder()
        {

        }

        public Task<ClientDeliveryAddressPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
