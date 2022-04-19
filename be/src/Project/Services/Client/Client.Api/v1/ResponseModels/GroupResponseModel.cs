using System;

namespace Client.Api.v1.ResponseModels
{
    public class GroupResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
