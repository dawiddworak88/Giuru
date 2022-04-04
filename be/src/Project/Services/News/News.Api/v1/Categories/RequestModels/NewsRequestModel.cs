using Foundation.ApiExtensions.Models.Request;
using News.Api.Infrastructure.Entities.News;
using System;
using System.Collections.Generic;

namespace News.Api.v1.Categories.RequestModels
{
    public class NewsRequestModel : RequestModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PreviewImageId { get; set; }
        public Guid? ThumbnailImageId { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
