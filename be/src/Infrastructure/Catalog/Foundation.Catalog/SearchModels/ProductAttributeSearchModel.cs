using Nest;

namespace Foundation.Catalog.SearchModels
{
    public class ProductAttributeSearchModel
    {
        [Keyword] public string Key { get; set; }
        [Keyword] public string Name { get; set; }
        [Keyword] public string[] ValueIds { get; set; }
        [Keyword] public string[] ValueKeywords { get; set; }
        [Text] public string ValueText { get; set; }
        public double? ValueNumeric { get; set; }
        public bool? ValueBoolean { get; set; }
    }
}
