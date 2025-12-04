namespace Foundation.ApiExtensions.Shared.Definitions
{
    public static class ApiConstants
    {
        public struct Analytics
        {
            public static readonly string SalesAnalyticsApiEndpoint = "/api/v1/salesanalytics";
            public static readonly string DailySalesAnalyticsApiEndpoint = "/api/v1/salesanalytics/daily";
            public static readonly string CountriesSalesAnalyticsApiEndpoint = "/api/v1/salesanalytics/countries";
            public static readonly string ProductsSalesAnalyticsApiEndpoint = "/api/v1/salesanalytics/products";
            public static readonly string ClientsSalesAnalyticsApiEndpoint = "/api/v1/salesanalytics/clients";
        }

        public struct Global
        {
            public static readonly string CountriesApiEndpoint = "/api/v1/countries";
            public static readonly string CurrenciesApiEndpoint = "/api/v1/currencies";
        }

        public struct Catalog
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string CategorySchemasApiEndpoint = "/api/v1/categories/categoryschemas";
            public static readonly string ProductsApiEndpoint = "/api/v1/products";
            public static readonly string ProductAttributesApiEndpoint = "/api/v1/productattributes";
            public static readonly string ProductAttributeItemsApiEndpoint = "/api/v1/productattributeitems";
            public static readonly string ProductsSearchIndexApiEndpoint = "/api/v1/productssearchindex";
            public static readonly string ProductSuggestionsApiEndpoint = "/api/v1/productsuggestions";
            public static readonly string ProductFilesApiEndpoint = "/api/v1/products/files";
            public static readonly string ProductsSkusApiEndpoint = "/api/v1/products/skus";
        }

        public struct Client
        {
            public static readonly string GroupsApiEndpoint = "/api/v1/clientgroups";
            public static readonly string RolesApiEndpoint = "/api/v1/clientroles";
            public static readonly string ApplicationsApiEndpoint = "/api/v1/clientsapplications";
            public static readonly string ManagersApiEndpoint = "/api/v1/clientaccountmanagers";
            public static readonly string ClientsApiEndpoint = "/api/v1/clients";
            public static readonly string ClientsByEmailApiEndpoint = "/api/v1/clients/email";
            public static readonly string AddressesApiEndpoint = "/api/v1/clientaddresses";
            public static readonly string FieldsApiEndpoint = "/api/v1/clientfields";
            public static readonly string FieldOptionsApiEndpoint = "/api/v1/clientfieldoptions";
            public static readonly string FieldValuesApiEndpoint = "/api/v1/clientfieldvalues";
        }

        public struct Identity
        {
            public static readonly string OrganisationsApiEndpoint = "/api/v1/organisations";
            public static readonly string SellersApiEndpoint = "/api/v1/sellers";
            public static readonly string ClientByOrganisationApiEndpoint = "/api/v1/clients/organisation";
            public static readonly string UsersApiEndpoint = "/api/v1/users";
            public static readonly string RolesApiEndpoint = "/api/v1/roles";
            public static readonly string TeamMembersEndpoint = "/api/v1/teammembers";
            public static readonly string ApprovalsEndpoint = "/api/v1/approvals";
            public static readonly string UserApprovalsEndpoint = "/api/v1/userapprovals";
        }

        public struct DownloadCenter
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string DownloadCenterApiEndponint = "/api/v1/downloadcenter";
            public static readonly string DownloadCenterCategoriesApiEndpoint = "/api/v1/downloadcenter/categories";
            public static readonly string DownloadCenterCategoryFilesApiEndpoint = "/api/v1/downloadcenter/categories/files";
        }

        public struct News
        {
            public static readonly string CategoriesApiEndpoint = "/api/v1/categories";
            public static readonly string NewsApiEndpoint = "/api/v1/news";
            public static readonly string NewsFilesApiEndpoint = "/api/v1/news/files";
        }

        public struct Inventory
        {
            public static readonly string WarehousesApiEndpoint = "/api/v1/warehouse";
            public static readonly string InventoryApiEndpoint = "/api/v1/inventory";
            public static readonly string InventoryProductsApiEndpoint = "/api/v1/inventory/products";
            public static readonly string AvailableProductsApiEndpoint = "/api/v1/inventory/availableproducts";
        }

        public struct Outlet
        {
            public static readonly string OutletApiEndpoint = "/api/v1/outlet";
            public static readonly string OutletProductsApiEndpoint = "/api/v1/outlet/products";
            public static readonly string AvailableOutletProductsApiEndpoint = "/api/v1/outlet/availableproducts";
            public static readonly string ProductOutletApiEndpoint = "/api/v1/outlet/product";
            public static readonly string ProductOutletBySkuApiEndpoint = "/api/v1/outlet/product/sku";
        }

        public struct Media
        {
            public static readonly string MediaItemsApiEndpoint = "/api/v1/mediaitems";
            public static readonly string MediaItemsVersionsApiEndpoint = "/api/v1/mediaitems/versions";
            public static readonly string FilesApiEndpoint = "/api/v1/files";
            public static readonly string FileChunksApiEndpoint = "/api/v1/files/chunks";
            public static readonly string FileChunksSaveCompleteApiEndpoint = "/api/v1/files/chunkssavecomplete";
        }

        public struct Baskets
        {
            public static readonly string BasketsApiEndpoint = "/api/v1/baskets";
            public static readonly string BasketsCheckoutApiEndpoint = "/api/v1/baskets/checkout";
        }

        public struct Order
        {
            public static readonly string OrdersApiEndpoint = "/api/v1/orders";
            public static readonly string OrderItemsApiEndpoint = "/api/v1/orders/orderitems";
            public static readonly string OrderItemStatusesApiEndpoint = "/api/v1/orders/orderitemstatuses";
            public static readonly string OrderStatusesApiEndpoint = "/api/v1/orderstatuses";
            public static readonly string UpdateOrderStatusApiEndpoint = "/api/v1/orders/orderstatus";
            public static readonly string UpdateOrderItemStatusApiEndpoint = "/api/v1/orders/orderitemstatus";
            public static readonly string OrderFilesApiEndpoint = "/api/v1/orders/files";
        }

        public struct ContentNames
        {
            public static readonly string FileContentName = "file";
            public static readonly string LanguageContentName = "language";
            public static readonly string GuidContentName = "id";
            public static readonly string ChunkNumberContentName = "chunkNumber";
            public static readonly string UploadIdContentName = "uploadId";
        }

        public struct Grula
        {
            public static readonly string PriceApiEndpoint = "/api/v1/price";
            public static readonly string PricesApiEndpoint = "/api/v1/prices";
        }

        public struct Request
        {
            public const long RequestSizeLimit = 250_000_000;
        }
    }
}
