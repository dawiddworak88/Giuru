using News.Api.ServicesModels.News;
using System;
using System.Collections.Generic;

namespace News.Api.v1.News.ResponseModels
{
    public class NewsItemResponseModel
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public bool IsNew { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
