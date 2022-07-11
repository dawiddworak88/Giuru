using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DownloadCenter.Api.Infrastructure.Entities.DownloadCenter
{
    public class DownloadCenterItem : Entity
    {
        [Required]
        public Guid CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
