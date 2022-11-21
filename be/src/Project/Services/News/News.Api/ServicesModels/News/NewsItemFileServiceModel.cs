using System;

namespace News.Api.ServicesModels.News
{
    public class NewsItemFileServiceModel
    {
        public Guid Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
