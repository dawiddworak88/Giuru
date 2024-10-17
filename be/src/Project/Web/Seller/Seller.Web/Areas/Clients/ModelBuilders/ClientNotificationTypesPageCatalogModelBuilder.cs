using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.NotificationTypes;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Threading.Tasks;
using Foundation.Localization;
using Foundation.GenericRepository.Definitions;
using System.Globalization;
using System.Collections.Generic;
using Foundation.Extensions.ExtensionMethods;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientNotificationTypesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientNotificationType>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer _globalLocalizer;
        private readonly IStringLocalizer _clientLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientNotificationTypesRepository _clientNotificationTypesRepository;

        public ClientNotificationTypesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator,
            IClientNotificationTypesRepository clientNotificationTypesRepository)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _linkGenerator = linkGenerator;
            _clientNotificationTypesRepository = clientNotificationTypesRepository;
        }

        public async Task<CatalogViewModel<ClientNotificationType>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<ClientNotificationType>, ClientNotificationType>();

            viewModel.Title = _clientLocalizer.GetString("NotificationTypes");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = _clientLocalizer.GetString("NewNotificationType");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "ClientNotificationType", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "ClientNotificationType", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "ClientNotificationTypesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ClientNotificationTypeApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(ClientNotificationType.CreatedDate)} desc";

            viewModel.ConfirmationDialogDeleteNameProperty = new List<string>
            {
                nameof(ClientNotificationType.Name).ToCamelCase()
            };

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("Name"),
                    _globalLocalizer.GetString("LastModifiedDate"),
                    _globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientNotificationType.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientNotificationType.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientNotificationType.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await _clientNotificationTypesRepository.GetAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(ClientNotificationType.CreatedDate)} desc");

            return viewModel;
        }
    }
}
