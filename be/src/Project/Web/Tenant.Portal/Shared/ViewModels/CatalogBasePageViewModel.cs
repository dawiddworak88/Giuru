namespace Tenant.Portal.Shared.ViewModels
{
    public class CatalogBasePageViewModel : BasePageViewModel
    {
        public string Locale { get; set; }
        public string EditLabel { get; set; }
        public string DeleteLabel { get; set; }
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
        public string SearchLabel { get; set; }
        public string DisplayedRowsLabel { get; set; }
        public string RowsPerPageLabel { get; set; }
        public string BackIconButtonText { get; set; }
        public string NextIconButtonText { get; set; }
        public string NoResultsLabel { get; set; }
    }
}
