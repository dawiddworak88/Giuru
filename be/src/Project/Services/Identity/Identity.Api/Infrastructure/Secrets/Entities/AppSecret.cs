using Foundation.GenericRepository.Entities;
using System;

namespace Identity.Api.Infrastructure.Secrets.Entities
{
    public class AppSecret : Entity
    {
        public Guid Secret { get; set; }
    }
}
