using System;

namespace Foundation.Search.SearchResultModels
{
    public class SearchModelBase
    {
        public string Language { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
