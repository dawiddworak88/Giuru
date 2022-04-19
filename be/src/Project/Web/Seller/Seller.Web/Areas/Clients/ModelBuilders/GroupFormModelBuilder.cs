using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class GroupFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, GroupFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;

        public GroupFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
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
                NavigateToGroupsText = this.clientLocalizer.GetString("NavigateToGroupsText")
            };

            return viewModel;
        }
    }
}