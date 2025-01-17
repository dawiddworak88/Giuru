using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Approvals;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ApprovalsPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Approval>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IApprovalsRepository _approvalsRepository;
        private readonly LinkGenerator _linkGenerator;

        public ApprovalsPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IApprovalsRepository approvalsRepository,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _approvalsRepository = approvalsRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<Approval>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<Approval>, Approval>();

            viewModel.Title = _globalLocalizer.GetString("ClientApprovals");
            viewModel.DefaultItemsPerPage = PaginationConstants.DefaultPageIndex;

            viewModel.NewText = _clientLocalizer.GetString("NewClientApproval");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "Approval", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "Approval", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "ApprovalsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ApprovalsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Approval.CreatedDate)} desc";

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
                        Title = nameof(Approval.Name).ToCamelCase(),
                        IsDateTime = false,
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Approval.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Approval.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await _approvalsRepository.GetAsync(componentModel.Token, componentModel.Language, null, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, $"{nameof(Approval.CreatedDate)} desc");

            return viewModel;
        }
    }
}
