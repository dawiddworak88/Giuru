using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories
{
    public class DownloadCenterCategoryFile : EntityMedia
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Filename { get; set; }
    }
}
