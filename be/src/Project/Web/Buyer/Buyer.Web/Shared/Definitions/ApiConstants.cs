namespace Buyer.Web.Shared.Definitions
{
    public static class ApiConstants
    {
        public struct Catalog
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string ProductsApiEndpoint = "/api/v1/products";
            public static readonly string ProductSuggestionsApiEndpoint = "/api/v1/productsuggestions";
        }

        public struct Seller
        {
            public static readonly string SellersApiEndpoint = "/api/v1/sellers";
        }

        public struct Media
        {
            public static readonly string MediaItemsApiEndpoint = "/api/v1/mediaitems";
        }
    }
}
