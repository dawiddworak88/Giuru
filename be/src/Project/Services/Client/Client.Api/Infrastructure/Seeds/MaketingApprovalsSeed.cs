using System.Linq;
using System;
using System.Linq.Dynamic.Core;
using Client.Api.Infrastructure.Clients.Entities;
using Foundation.GenericRepository.Extensions;
using Client.Api.Definitions;

namespace Client.Api.Infrastructure.Seeds
{
    public static class MaketingApprovalsSeed
    {
        public static void SeedMarketingApprovals(ClientContext context)
        {
            SeedMarketingApproval(context, MarketingApporvalConstants.EmailMarketingApprovalName, true);
            SeedMarketingApproval(context, MarketingApporvalConstants.SmsMarketingApprovalName, true);
        }

        private static void SeedMarketingApproval(ClientContext context, string name, bool isApproved) 
        {
            if (!context.ClientMarketingApprovals.Any())
            {
                var marketingApprovals = new ClientMarketingApproval
                {
                    Name = name,
                    IsApproved = isApproved
                };

                context.ClientMarketingApprovals.Add(marketingApprovals.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
