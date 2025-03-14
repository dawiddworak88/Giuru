﻿using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ApiResponseModels
{
    public class NewsItemResponseModel
    {
        public Guid Id { get; set; }
        public Guid? ThumbnailImageId { get; set; }
        public Guid? PreviewImageId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
