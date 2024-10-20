﻿using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.TeamMembers.ApiRequestModel
{
    public class TeamMemberApiRequestModel : RequestModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public string ReturnUrl { get; set; }
    }
}
