using System;

namespace Foundation.ApiExtensions.Models.Request
{
    public class RequestModelBase
    {
        public Guid? Id { get; set; }
        public string Language { get; set; }
    }
}
