using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Client.Api.Infrastructure.Groups.Entities
{
    public class ClientGroup : Entity
    {
        public virtual IEnumerable<ClientGroupTranslations> Translations { get; set; }
    }
}
