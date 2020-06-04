namespace Tenant.Portal.Shared.ViewModels
{
    public class CatalogBasePageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public bool ShowNew { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
    }
}
