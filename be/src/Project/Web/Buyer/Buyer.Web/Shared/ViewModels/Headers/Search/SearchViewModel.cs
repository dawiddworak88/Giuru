namespace Buyer.Web.Shared.ViewModels.Headers.Search
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public string SearchUrl { get; set; }
        public string SearchLabel { get; set; }
        public string SearchPlaceholderLabel { get; set; }
        public string GetSuggestionsUrl { get; set; }
        public string NoResultText { get; set; }
        public string ChangeSearchTermText { get; set; }
    }
}
