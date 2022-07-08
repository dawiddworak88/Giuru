using Foundation.GenericRepository.Entities;
using System;

namespace DownloadCenter.Api.Infrastructure.Entities.Downloads
{
    public class Download : Entity
    {
        public Guid CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
