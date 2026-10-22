using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class DiscountCodeFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodeFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IDiscountCodesRepository _discountCodesRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly LinkGenerator _linkGenerator;

        public DiscountCodeFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IDiscountCodesRepository discountCodesRepository,
            IClientsRepository clientsRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _discountCodesRepository = discountCodesRepository;
            _clientsRepository = clientsRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<DiscountCodeFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DiscountCodeFormViewModel
            {
                Title = _clientLocalizer.GetString("EditDiscountCode"),
                IdLabel = _globalLocalizer.GetString("Id"),
                CodeLabel = _clientLocalizer.GetString("DiscountCode"),
                DescriptionLabel = _globalLocalizer.GetString("Description"),
                ClientsLabel = _clientLocalizer.GetString("AssignedClients"),
                ActiveLabel = _globalLocalizer.GetString("Active"),
                InActiveLabel = _globalLocalizer.GetString("InActive"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                CodeRequiredErrorMessage = _clientLocalizer.GetString("DiscountCodeRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToDiscountCodesText = _clientLocalizer.GetString("NavigateToDiscountCodesText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "DiscountCodesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                DiscountCodesUrl = _linkGenerator.GetPathByAction("Index", "DiscountCodes", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            // Disabled Clients stay assignable: they keep their assignments and the Seller can still order for them.
            var clients = await _clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language);

            if (clients is not null)
            {
                viewModel.Clients = clients.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var discountCode = await _discountCodesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (discountCode is not null)
                {
                    viewModel.Id = discountCode.Id;
                    viewModel.Code = discountCode.Code;
                    viewModel.Description = discountCode.Description;
                    viewModel.IsDisabled = discountCode.IsDisabled;
                    viewModel.ClientIds = discountCode.ClientIds ?? viewModel.ClientIds;
                }
            }

            return viewModel;
        }
    }
}
