using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Client.Api.Infrastructure.Fields
{
    public class OptionSet : Entity
    {
        public virtual IEnumerable<OptionSetTranslation> OptionSetTranslations { get; set; }
    }
}
