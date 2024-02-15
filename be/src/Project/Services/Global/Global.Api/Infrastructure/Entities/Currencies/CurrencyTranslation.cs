using Foundation.GenericRepository.Entities;
using System;

namespace Global.Api.Infrastructure.Entities.Currencies
{
    public class CurrencyTranslation : Entity
    {
        public Guid? CurrencyId { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
    }
}
