using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class CatalogModelBuilder : ICatalogModelBuilder
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public CatalogModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _globalLocalizer = globalLocalizer;
        }

        public T BuildModel<T, W>() where T : CatalogViewModel<W>, new() where W: class
        {
            var viewModel = new T
            {
                NoLabel = _globalLocalizer.GetString("No"),
                YesLabel = _globalLocalizer.GetString("Yes"),
                DeleteConfirmationLabel = _globalLocalizer.GetString("DeleteConfirmationLabel"),
                AreYouSureLabel = _globalLocalizer.GetString("AreYouSureLabel"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SearchLabel = _globalLocalizer.GetString("Search"),
                EditLabel = _globalLocalizer.GetString("Edit"),
                DeleteLabel = _globalLocalizer.GetString("Delete"),
                DuplicateLabel = _globalLocalizer.GetString("Duplicate"),
                DragLabel = _globalLocalizer.GetString("Drag"),
                DisplayedRowsLabel = _globalLocalizer.GetString("DisplayedRows"),
                RowsPerPageLabel = _globalLocalizer.GetString("RowsPerPage"),
                NoResultsLabel = _globalLocalizer.GetString("NoResultsLabel"),
                GenerateQRCodeLabel = _globalLocalizer.GetString("GenerateQRCode"),
                CopyLinkLabel = _globalLocalizer.GetString("CopyLink"),
                InActiveLabel = _globalLocalizer.GetString("InActive"),
                ActiveLabel = _globalLocalizer.GetString("Active"),
                SearchTerm = string.Empty
            };

            return viewModel;
        }
    }
}