using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.Repositories.FieldOptions;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientFieldOptionFormModelBuilder : IAsyncComponentModelBuilder<ClientFieldOptionComponentModel, ClientFieldOptionFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientFieldOptionsRepository _clientFieldOptionsRepository;
        private readonly LinkGenerator _linkGenerator;

        public ClientFieldOptionFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientFieldOptionsRepository clientFieldOptionsRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _linkGenerator = linkGenerator;
            _clientFieldOptionsRepository = clientFieldOptionsRepository;
        }

        public async Task<ClientFieldOptionFormViewModel> BuildModelAsync(ClientFieldOptionComponentModel componentModel)
        {
            var viewModel = new ClientFieldOptionFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _clientLocalizer.GetString("EditClientFieldOption"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NameLabel = _globalLocalizer.GetString("Name"),
                ValueLabel = _globalLocalizer.GetString("Value"),
                NavigateToFieldText = _clientLocalizer.GetString("BackToField"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientFieldsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            var fieldDefinitionId = componentModel.ClientFieldOptionId;

            if (componentModel.Id.HasValue)
            {
                viewModel.Id = componentModel.Id;

                var fieldOption = await _clientFieldOptionsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (fieldOption is not null)
                {
                    viewModel.Name = fieldOption.Name;
                    viewModel.Value = fieldOption.Value;
                    fieldDefinitionId = fieldOption.FieldDefinitionId;

                }
            }

            viewModel.FieldDefinitionId = fieldDefinitionId;
            viewModel.ClientFieldUrl = _linkGenerator.GetPathByAction("Edit", "ClientField", new { Id = fieldDefinitionId, Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            return viewModel;
        }
    }
}
