using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace News.Api.ServicesModels.News
{
    public class CreateNewsItemServiceModel : BaseServiceModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public bool IsNew { get; set; }
        public Guid? CategoryId { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public IEnumerable<Guid> Images { get; set; }
    }
}
