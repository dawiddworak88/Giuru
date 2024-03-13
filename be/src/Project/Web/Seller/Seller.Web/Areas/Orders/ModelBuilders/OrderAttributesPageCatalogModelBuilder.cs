using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.OrderAttributes;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OrderAttribute>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOrderAttributesRepository _orderAttributesRepository;
        private readonly LinkGenerator _linkGenerator;

        public OrderAttributesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder, 
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<OrderResources> orderLocalizer, 
            IOrderAttributesRepository orderAttributesRepository,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _orderAttributesRepository = orderAttributesRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<OrderAttribute>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<OrderAttribute>, OrderAttribute>();

            viewModel.Title = _globalLocalizer.GetString("OrderAttributes");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = _orderLocalizer.GetString("NewOrderAttribute");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "OrderAttribute", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "OrderAttribute", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "OrderAttributesApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(OrderAttribute.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("Name"),
                    _globalLocalizer.GetString("Type"),
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
                        Title = nameof(OrderAttribute.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OrderAttribute.Type).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OrderAttribute.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OrderAttribute.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await _orderAttributesRepository.GetAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(OrderAttribute.CreatedDate)} desc");

            return viewModel;
        }
    }
}
