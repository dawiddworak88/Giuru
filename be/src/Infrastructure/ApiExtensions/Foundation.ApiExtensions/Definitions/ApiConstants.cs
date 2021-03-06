namespace Foundation.ApiExtensions.Shared.Definitions
{
    public static class ApiConstants
    {
        public struct Catalog
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string CategorySchemasApiEndpoint = "/api/v1/categories/categoryschemas";
            public static readonly string ProductsApiEndpoint = "/api/v1/products";
            public static readonly string ProductAttributesApiEndpoint = "/api/v1/productattributes";
            public static readonly string ProductAttributeItemsApiEndpoint = "/api/v1/productattributeitems";
            public static readonly string ProductsSearchIndexApiEndpoint = "/api/v1/productssearchindex";
            public static readonly string ProductSuggestionsApiEndpoint = "/api/v1/productsuggestions";
        }

        public struct Identity
        {
            public static readonly string OrganisationsApiEndpoint = "/api/v1/organisations";
            public static readonly string SellersApiEndpoint = "/api/v1/sellers";
            public static readonly string ClientsApiEndpoint = "/api/v1/clients";
        }

        public struct Media
        {
            public static readonly string MediaItemsApiEndpoint = "/api/v1/mediaitems";
            public static readonly string FilesApiEndpoint = "/api/v1/files";
        }

        public struct Baskets
        {
            public static readonly string BasketsApiEndpoint = "/api/v1/baskets";
            public static readonly string BasketsCheckoutApiEndpoint = "/api/v1/baskets/checkout";
        }

        public struct Order
        {
            public static readonly string OrdersApiEndpoint = "/api/v1/orders";
            public static readonly string OrderStatusesApiEndpoint = "/api/v1/orderstatuses";
            public static readonly string UpdateOrderStatusApiEndpoint = "/api/v1/orders/orderstatus";
        }

        public struct ContentNames
        {
            public static readonly string FileContentName = "file";
            public static readonly string LanguageContentName = "language";
        }

        public struct Request
        {
            public const long RequestSizeLimit = 200_000_000;
        }
    }
}
