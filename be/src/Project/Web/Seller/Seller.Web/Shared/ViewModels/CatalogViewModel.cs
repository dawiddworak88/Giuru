﻿using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Seller.Web.Shared.ViewModels
{
    public class CatalogViewModel<T> where T: class
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
        public string SearchApiUrl { get; set; }
        public string EditLabel { get; set; }
        public string DeleteLabel { get; set; }
        public string DuplicateLabel { get; set; }
        public string SearchLabel { get; set; }
        public string SearchTerm { get; set; }
        public string DisplayedRowsLabel { get; set; }
        public string RowsPerPageLabel { get; set; }
        public string NoResultsLabel { get; set; }
        public string NoLabel { get; set; }
        public string YesLabel { get; set; }
        public string DeleteConfirmationLabel { get; set; }
        public string AreYouSureLabel { get; set; }
        public string DeleteApiUrl { get; set; }
        public string EditUrl { get; set; }
        public string DuplicateUrl { get; set; }
        public string IsAttachmentLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string OrderBy { get; set; }
        public IEnumerable<string> ConfirmationDialogDeleteNameProperty { get; set; }
        public CatalogTableViewModel Table { get; set; }
        public QRCodeDialogViewModel QrCodeDialog { get; set; }
        public PagedResults<IEnumerable<T>> PagedItems { get; set; }
    }
}
