using Feature.Client;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.ModelBuilders
{
    public class ClientCatalogModelBuilder : IModelBuilder<ClientCatalogViewModel>
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ClientCatalogModelBuilder(
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public ClientCatalogViewModel BuildModel()
        {
            return new ClientCatalogViewModel
            {
                Title = this.clientLocalizer["Clients"],
                ShowNew = true,
                NewText = this.clientLocalizer["NewClient"],
                NewUrl = this.linkGenerator.GetPathByAction("Index", "ClientDetail", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
            };
        }
    }
}
