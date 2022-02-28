using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace News.Api.Infrastructure.Entities.Tags
{
    public class Tag : Entity
    {
        public virtual IEnumerable<TagTranslation> Translations { get; set; }
    }
}
