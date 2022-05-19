using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Localization;
using Microsoft.Extensions.Localization;

namespace Buyer.Web.Shared.ModelBuilders.Catalogs
{
    public class CatalogOrderModelBuilder : ICatalogOrderModelBuilder
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public CatalogOrderModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public T BuildModel<T, W>() where T : CatalogOrderViewModel<W>, new() where W : class
        {
            var viewModel = new T
            {
                NoLabel = this.globalLocalizer.GetString("No"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                DeleteConfirmationLabel = this.globalLocalizer.GetString("DeleteConfirmationLabel"),
                AreYouSureLabel = this.globalLocalizer.GetString("AreYouSureLabel"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SearchLabel = this.globalLocalizer.GetString("Search"),
                EditLabel = this.globalLocalizer.GetString("Edit"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                DuplicateLabel = this.globalLocalizer.GetString("Duplicate"),
                DisplayedRowsLabel = this.globalLocalizer.GetString("DisplayedRows"),
                RowsPerPageLabel = this.globalLocalizer.GetString("RowsPerPage"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResultsLabel"),
                SearchTerm = string.Empty
            };

            return viewModel;
        }
    }
}
