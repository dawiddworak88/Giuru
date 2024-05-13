namespace Catalog.Api.Configurations
{
    public class AppSettings
    {
        public string ElasticsearchIndex { get; set; }
        public string SupportedCultures { get; set; }
        public int AttributeLimit { get; set; }
    }
}
