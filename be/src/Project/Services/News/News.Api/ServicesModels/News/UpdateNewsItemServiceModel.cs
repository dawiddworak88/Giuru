using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace News.Api.ServicesModels.News
{
    public class UpdateNewsItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? HeroImageId { get; set; }
        public Guid? ThumbImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
