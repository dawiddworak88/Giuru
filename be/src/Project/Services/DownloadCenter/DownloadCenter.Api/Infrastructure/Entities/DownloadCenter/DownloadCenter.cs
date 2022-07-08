using Foundation.GenericRepository.Entities;
using System;

namespace DownloadCenter.Api.Infrastructure.Entities.DownloadCenter
{
    public class DownloadCenter : Entity
    {
        public Guid CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
