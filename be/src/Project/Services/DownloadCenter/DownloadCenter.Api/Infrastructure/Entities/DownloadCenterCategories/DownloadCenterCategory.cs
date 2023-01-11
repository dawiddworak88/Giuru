using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("ParentCategoryId")]
        public virtual DownloadCenterCategory ParentCategory { get; set; }

        public virtual IEnumerable<DownloadCenterCategoryFile> Files { get; set; }

        public virtual IEnumerable<DownloadCenterCategoryTranslation> Translations { get; set; }
    }
}