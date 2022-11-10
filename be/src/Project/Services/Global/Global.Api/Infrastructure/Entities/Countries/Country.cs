using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Global.Api.Infrastructure.Entities.Countries
{
    public class Country : Entity
    {
        public virtual IEnumerable<CountryTranslation> Translations { get; set; }
    }
}
