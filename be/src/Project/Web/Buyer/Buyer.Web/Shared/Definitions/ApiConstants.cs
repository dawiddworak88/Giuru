namespace Buyer.Web.Shared.Definitions
{
    public static class ApiConstants
    {
        public struct Catalog
        {
            public static readonly string CategoryApiEndpoint = "/api/v1/category";
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string ProductsApiEndpoint = "/api/v1/products";
            public static readonly string ProductApiEndpoint = "/api/v1/product";
            public static readonly string ProductSuggestionsApiEndpoint = "/api/v1/productsuggestions";
        }

        public struct Seller
        {
            public static readonly string SellerApiEndpoint = "/api/v1/seller";
        }

        public struct Media
        {
            public static readonly string MediaItemsApiEndpoint = "/api/v1/mediaitems";
        }
    }
}
