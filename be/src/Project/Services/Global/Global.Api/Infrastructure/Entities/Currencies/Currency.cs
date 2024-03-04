using Foundation.GenericRepository.Entities;
using Global.Api.Infrastructure.Entities.Countries;
using System;
using System.Collections.Generic;

namespace Global.Api.Infrastructure.Entities.Currencies
{
    public class Currency : Entity
    {
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public virtual IEnumerable<CurrencyTranslation> Translations { get; set; }
    }
}
