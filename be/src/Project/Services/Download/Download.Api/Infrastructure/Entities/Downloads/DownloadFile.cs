using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Download.Api.Infrastructure.Entities.Downloads
{
    public class DownloadFile : EntityMedia
    {
        [Required]
        public Guid DownloadId { get; set; }
    }
}
