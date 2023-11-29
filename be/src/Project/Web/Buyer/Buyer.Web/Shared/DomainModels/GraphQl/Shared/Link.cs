namespace Buyer.Web.Shared.DomainModels.GraphQl.Shared
{
    public class Link
    {
        public string Href { get; set; }
        public string Label { get; set; }
        public string Target { get; set; }
        public bool IsExternal { get; set; }
    }
}
