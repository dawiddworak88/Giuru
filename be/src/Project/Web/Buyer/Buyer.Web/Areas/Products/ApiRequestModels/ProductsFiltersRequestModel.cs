namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class ProductsFiltersRequestModel
    {
        public string Ids { get; set; }
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string Source { get; set; }
        public string OrderBy { get; set; }
    }
}
