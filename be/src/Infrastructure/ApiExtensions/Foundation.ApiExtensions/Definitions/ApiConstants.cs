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

        public struct Client
        {
            public static readonly string GroupsApiEndpoint = "/api/v1/clientgroups";
            public static readonly string RolesApiEndpoint = "/api/v1/clientroles";
            public static readonly string ApplicationsApiEndpoint = "/api/v1/clientsapplication";
            public static readonly string ManagersApiEndpoint = "/api/v1/clientaccountmanagers";
        }

        public struct Identity
        {
            public static readonly string OrganisationsApiEndpoint = "/api/v1/organisations";
            public static readonly string SellersApiEndpoint = "/api/v1/sellers";
            public static readonly string ClientsApiEndpoint = "/api/v1/clients";
            public static readonly string ClientByOrganisationApiEndpoint = "/api/v1/clients/organisation";
            public static readonly string UsersApiEndpoint = "/api/v1/users";
            public static readonly string RolesApiEndpoint = "/api/v1/roles";
        }

        public struct DownloadCenter
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string DownloadCenterApiEndponint = "/api/v1/downloadcenter";
            public static readonly string DownloadCenterCategoryApiEndpoint = "/api/v1/downloadcenter/categories";
        }

        public struct News
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string NewsApiEndpoint = "/api/v1/news";
        }

        public struct Inventory
        {
            public static readonly string WarehousesApiEndpoint = "/api/v1/warehouse";
            public static readonly string InventoryApiEndpoint = "/api/v1/inventory";
            public static readonly string AvailableProductsApiEndpoint = "/api/v1/inventory/availableproducts";
        }

        public struct Outlet
        {
            public static readonly string OutletApiEndpoint = "/api/v1/outlet";
            public static readonly string AvailableOutletProductsApiEndpoint = "/api/v1/outlet/availableproducts";
            public static readonly string ProductOutletApiEndpoint = "/api/v1/outlet/product";
        }

        public struct Media
        {
            public static readonly string MediaItemsApiEndpoint = "/api/v1/mediaitems";
            public static readonly string MediaItemsVersionsApiEndpoint = "/api/v1/mediaitems/versions";
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
            public static readonly string GuidContentName = "id";
        }

        public struct Request
        {
            public const long RequestSizeLimit = 250_000_000;
        }
    }
}
