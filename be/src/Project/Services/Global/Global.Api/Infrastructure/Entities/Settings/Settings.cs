using Foundation.GenericRepository.Entities;
using System;

namespace Global.Api.Infrastructure.Entities.Settings
{
    public class Setting : Entity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid SellerId { get; set; }
    }
}