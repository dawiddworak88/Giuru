using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Database.Shared.Secrets.Entities
{
    public class AppSecret : Entity
    {
        public Guid Secret { get; set; }
    }
}
