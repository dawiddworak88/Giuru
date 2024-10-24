using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Identity.Api.Infrastructure.Approvals.Entities
{
    public class Approval : Entity
    {
        public virtual IEnumerable<ApprovalTranslation> Translations { get; set; }
    }
}
