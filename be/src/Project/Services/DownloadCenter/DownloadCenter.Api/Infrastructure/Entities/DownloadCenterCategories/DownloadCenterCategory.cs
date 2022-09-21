using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories
{
    public class DownloadCenterCategory : Entity
    {
        public Guid? ParentCategoryId { get; set; }
        public int? Order { get; set; }

        [Required]
        public bool IsVisible { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        public virtual IEnumerable<DownloadCenterCategoryTranslation> Translations { get; set; }
    }
}