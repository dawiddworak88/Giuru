using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories
{
    public class DownloadCenterFilesGroup : Entity
    {
        [Required]
        public Guid MediaItemId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
