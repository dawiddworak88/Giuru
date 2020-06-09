using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Shared.Catalogs.ModelBuilders;
using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class CatalogModelBuilder : ICatalogModelBuilder
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public CatalogModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public T BuildModel<T>() where T : CatalogBaseViewModel, new()
        {
            var viewModel = new T
            {
                NoLabel = this.globalLocalizer["No"],
                YesLabel = this.globalLocalizer["Yes"],
                DeleteConfirmationLabel = this.globalLocalizer["DeleteConfirmationLabel"],
                AreYouSureLabel = this.globalLocalizer["AreYouSureLabel"],
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"],
                SearchLabel = this.globalLocalizer["Search"],
                EditLabel = this.globalLocalizer["Edit"],
                DeleteLabel = this.globalLocalizer["Delete"],
                DisplayedRowsLabel = this.globalLocalizer["DisplayedRows"],
                RowsPerPageLabel = this.globalLocalizer["RowsPerPage"],
                BackIconButtonText = this.globalLocalizer["Previous"],
                NextIconButtonText = this.globalLocalizer["Next"],
                NoResultsLabel = this.globalLocalizer["NoResultsLabel"]
            };

            return viewModel;
        }
    }
}