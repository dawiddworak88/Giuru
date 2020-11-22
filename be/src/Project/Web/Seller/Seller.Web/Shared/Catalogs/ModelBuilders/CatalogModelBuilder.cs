using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class CatalogModelBuilder : ICatalogModelBuilder
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public CatalogModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public T BuildModel<T, W>() where T : CatalogViewModel<W>, new() where W: class
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
                NoResultsLabel = this.globalLocalizer["NoResultsLabel"],
                LastModifiedDateLabel = this.globalLocalizer["LastModifiedDate"],
                CreatedDateLabel = this.globalLocalizer["CreatedDate"]
        };

            return viewModel;
        }
    }
}