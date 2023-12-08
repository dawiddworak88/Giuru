using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.FieldOptions
{
    public class ClientFieldOptionServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public IEnumerable<FieldOptionServiceModel> Options { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
