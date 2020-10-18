using Foundation.GenericRepository.Entities;
using System;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class OrganisationTranslation : EntityTranslation
    {
        public string Description { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
