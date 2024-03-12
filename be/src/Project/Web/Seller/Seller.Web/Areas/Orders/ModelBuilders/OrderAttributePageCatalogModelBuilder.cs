using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributePageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OrderAttributeOption>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public OrderAttributePageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<OrderAttributeOption>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<OrderAttributeOption>, OrderAttributeOption>();

            viewModel.NewText = _orderLocalizer.GetString("NewAttributeOption");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("New", "OrderAttributeOption", new { Id = componentModel.Id, Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "OrderAttributeOption", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "ClientFieldOptionsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ClientFieldOptionsApi", new { FieldDefinitionId = componentModel.Id, Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(OrderAttributeOption.CreatedDate)} desc";
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.ConfirmationDialogDeleteNameProperty = new List<string>
            {
                nameof(OrderAttributeOption.Name).ToCamelCase()
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
                        Title = nameof(OrderAttributeOption.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OrderAttributeOption.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OrderAttributeOption.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            return viewModel;
        }
    }
}
