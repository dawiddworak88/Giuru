using Foundation.GenericRepository.Entities;
using System;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class OrganisationFile : EntityMedia
    {
        public Guid OrganisationId { get; set; }
    }
}
