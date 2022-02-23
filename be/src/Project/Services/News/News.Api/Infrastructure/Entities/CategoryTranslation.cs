
using Foundation.GenericRepository.Entities;
using System;

namespace News.Api.Infrastructure.Entities
{
    public class CategoryTranslation : Entity
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public Guid CategoryId { get; set; }
    }
}
