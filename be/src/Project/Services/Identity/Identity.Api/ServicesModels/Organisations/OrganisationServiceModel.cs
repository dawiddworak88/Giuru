using System;
using System.Collections.Generic;

namespace Identity.Api.ServicesModels.Organisations
{
    public class OrganisationServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public string Description { get; set; }
    }
}
