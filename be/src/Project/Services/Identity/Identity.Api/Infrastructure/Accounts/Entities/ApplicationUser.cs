﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;

namespace Identity.Api.Infrastructure.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? ExpirationId { get; set; }
        public DateTime? VerifyExpirationDate { get; set; }
        public Guid OrganisationId { get; set; }
        public bool IsDisabled { get; set; }
    }
}
