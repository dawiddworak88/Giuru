namespace Foundation.Catalog.SearchModels
{
    public class ProductAttributeSearchModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string[] ValueIds { get; set; }
        public string[] ValueKeywords { get; set; }
        public string ValueText { get; set; }
        public double? ValueNumeric { get; set; }
        public bool? ValueBoolean { get; set; }
    }
}
