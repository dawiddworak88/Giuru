using System.Collections.Generic;

namespace Buyer.Web.Shared.Catalogs.ViewModels
{
    public class CatalogViewModel
    {
        public string Title { get; set; }
        public int ResultsCount { get; set; }
        public string ResultsLabel { get; set; }
        public string NoResultsLabel { get; set; }
        public string SkuLabel { get; set; }
        public string ByLabel { get; set; }
        public string InStockLabel { get; set; }
        public bool IsAuthenticated { get; set; }
        public string SignInUrl { get; set; }
        public string SignInToSeePricesLabel { get; set; }
        public IEnumerable<CatalogItemViewModel> Items { get; set; }
    }
}
