using Foundation.GenericRepository.Entities;
using System;

namespace Media.Api.Infrastructure.Media.Entities
{
    public class MediaItemTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }
        public Guid MediaItemVersionId { get; set; }
    }
}
