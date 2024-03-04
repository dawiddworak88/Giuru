using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class Option : Entity
    {
        [Required]
        public Guid OptionSetId { get; set; }

        public virtual OptionSet OptionSet { get; set; }

        public virtual IEnumerable<OptionTranslation> OptionsTranslations { get; set; }
    }
}
