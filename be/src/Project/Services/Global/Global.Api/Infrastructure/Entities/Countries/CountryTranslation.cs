using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Global.Api.Infrastructure.Entities.Countries
{
    public class CountryTranslation : EntityTranslation
    {
        [Required]
        public Guid CountryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
