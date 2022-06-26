using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientGroupFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IClientGroupsRepository clientGroupsRepository;

        public ClientGroupFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientGroupsRepository clientGroupsRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.clientGroupsRepository = clientGroupsRepository;
        }

        public async Task<ClientGroupFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientGroupFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditGroup"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("NameRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToGroupsText = this.clientLocalizer.GetString("NavigateToGroupsText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ClientGroupsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GroupsUrl = this.linkGenerator.GetPathByAction("Index", "ClientGroups", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var group = await this.clientGroupsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);
                if (group is not null)
                {
                    viewModel.Id = group.Id;
                    viewModel.Name = group.Name;
                }
            }

            return viewModel;
        }
    }
}