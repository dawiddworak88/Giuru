namespace Foundation.ApiExtensions.Shared.Definitions
{
    public static class ApiConstants
    {
        public struct Catalog
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string ProductsApiEndpoint = "/api/v1/products";
            public static readonly string ProductSuggestionsApiEndpoint = "/api/v1/productsuggestions";
        }

        public struct Identity
        {
            public static readonly string SellersApiEndpoint = "/api/v1/sellers";
        }

        public struct Media
        {
            public static readonly string MediaItemsApiEndpoint = "/api/v1/mediaitems";
            public static readonly string FilesApiEndpoint = "/api/v1/files";
        }

        public struct ContentNames
        {
            public static readonly string FileContentName = "file";
            public static readonly string LanguageContentName = "language";
        }
    }
}
