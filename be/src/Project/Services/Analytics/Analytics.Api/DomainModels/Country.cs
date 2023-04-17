using System;

namespace Analytics.Api.DomainModels
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
