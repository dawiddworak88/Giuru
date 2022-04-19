using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Client.Api.Infrastructure.Groups.Entities
{
    public class Group : Entity
    {
        public virtual IEnumerable<GroupTranslation> Translations { get; set; }
    }
}
