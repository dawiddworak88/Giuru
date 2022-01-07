using Foundation.GenericRepository.Entities;
using System;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class OrganisationVideo : EntityMedia
    {
        public Guid OrganisationId { get; set; }
    }
}
