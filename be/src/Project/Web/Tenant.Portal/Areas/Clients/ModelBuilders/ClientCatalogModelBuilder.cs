using Feature.Client;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.ModelBuilders
{
    public class ClientCatalogModelBuilder : IModelBuilder<ClientCatalogViewModel>
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientCatalogModelBuilder(IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.clientLocalizer = clientLocalizer;
        }

        public ClientCatalogViewModel BuildModel()
        {
            return new ClientCatalogViewModel
            {
                Title = this.clientLocalizer["Clients"],
                ShowNew = true,
                NewText = this.clientLocalizer["NewClient"],
                NewUrl = "#"
            };
        }
    }
}
