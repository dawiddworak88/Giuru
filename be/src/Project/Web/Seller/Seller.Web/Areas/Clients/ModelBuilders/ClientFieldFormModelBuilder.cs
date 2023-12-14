using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.Fields;
using Seller.Web.Areas.Clients.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientFieldFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientFieldsRepository _clientFieldsRepository;
        private readonly LinkGenerator _linkGenerator;

        public ClientFieldFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientFieldsRepository clientFieldsRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _linkGenerator = linkGenerator;
            _clientFieldsRepository = clientFieldsRepository;
        }

        public async Task<ClientFieldFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientFieldFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _clientLocalizer.GetString("EditClientField"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NameLabel = _globalLocalizer.GetString("Name"),
                TypeLabel = _globalLocalizer.GetString("Type"),
                NavigateToFieldsText = _clientLocalizer.GetString("BackToFields"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                ClientFieldsUrl = _linkGenerator.GetPathByAction("Index", "ClientFields", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientFieldsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                EditUrl = _linkGenerator.GetPathByAction("Edit", "ClientField", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                Types = new List<ClientFieldTypeViewModel>()
                {
                    new() { Value = "text", Text = _globalLocalizer.GetString("String") },
                    new() { Value = "select", Text = _globalLocalizer.GetString("Array") },
                    new() { Value = "number", Text = _globalLocalizer.GetString("Number") },
                    new() { Value = "boolean", Text = _globalLocalizer.GetString("Boolean") }
                }
            };

            if (componentModel.Id.HasValue)
            {
                viewModel.Id = componentModel.Id;

                var clientField = await _clientFieldsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (clientField is not null)
                {
                    viewModel.Name = clientField.Name;
                    viewModel.Type = clientField.Type;
                }
            }

            return viewModel;
        }
    }
}
