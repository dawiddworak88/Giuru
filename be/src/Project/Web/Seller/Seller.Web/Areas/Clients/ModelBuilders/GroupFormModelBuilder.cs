using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class GroupFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, GroupFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IGroupsRepository groupsRepository;

        public GroupFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IGroupsRepository groupsRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.groupsRepository = groupsRepository;
        }

        public async Task<GroupFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new GroupFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditGroup"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("NameRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToGroupsText = this.clientLocalizer.GetString("NavigateToGroupsText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "GroupsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GroupsUrl = this.linkGenerator.GetPathByAction("Index", "Groups", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var group = await this.groupsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);
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