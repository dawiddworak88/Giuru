using System;

namespace Media.Api.Configurations
{
    public class AppSettings
    {
        public string StorageConnectionString { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
