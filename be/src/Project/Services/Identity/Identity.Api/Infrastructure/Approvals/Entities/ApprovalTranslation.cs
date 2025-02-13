using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Approvals.Entities
{
    public class ApprovalTranslation : EntityTranslation
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApprovalId { get; set; }
    }
}
