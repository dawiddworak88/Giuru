using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.ClientApprovals;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientApprovalsPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientApproval>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientApprovalsRepository _clientApprovalsRepository;
        private readonly LinkGenerator _linkGenerator;

        public ClientApprovalsPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientApprovalsRepository clientApprovalsRepository,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _clientApprovalsRepository = clientApprovalsRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<ClientApproval>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<ClientApproval>, ClientApproval>();

            viewModel.Title = _globalLocalizer.GetString("ClientApprovals");
            viewModel.DefaultItemsPerPage = PaginationConstants.DefaultPageIndex;

            viewModel.NewText = _clientLocalizer.GetString("NewClientApproval");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "ClientApproval", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "ClientApproval", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "ClientApprovalsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ClientApprovalsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(ClientApproval.CreatedDate)} desc";

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
                        IsDelete = true,
                    },
                    new CatalogActionViewModel
                    {
                        IsEdit = true,
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientApproval.Name).ToCamelCase(),
                        IsDateTime = false,
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientApproval.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientApproval.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await _clientApprovalsRepository.GetAsync(componentModel.Token, componentModel.Language, null, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, $"{nameof(ClientApproval.CreatedDate)} desc");

            return viewModel;
        }
    }
}
