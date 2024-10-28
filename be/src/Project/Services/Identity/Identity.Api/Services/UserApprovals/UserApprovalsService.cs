using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Approvals.Entities;
using Identity.Api.ServicesModels.UserApprovals;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Services.UserApprovals
{
    public class UserApprovalsService : IUserApprovalsService
    {
        private readonly IdentityContext _context;

        public UserApprovalsService(IdentityContext context)
        {
            _context = context;
        }

        public IEnumerable<UserApprovalServiceModel> Get(GetUserApprovalsServiceModel model)
        {
            var userApprovals = _context.UserApprovals.Where(x => x.UserId == model.UserId && x.IsActive);

            if (userApprovals is not null && userApprovals.Any())
            {
                return userApprovals.Select(x => new UserApprovalServiceModel
                {
                    UserId = x.UserId,
                    ApprovalId = x.ApprovalId,
                    CreatedDate = x.CreatedDate,
                });
            }

            return default;
        }

        public async Task SaveAsync(SaveUserApprovalsServiceModel model)
        {
            var userApprovals = _context.UserApprovals.Where(x => x.UserId == model.UserId && x.IsActive).ToList();

            foreach (var userApproval in userApprovals.OrEmptyIfNull())
            {
                _context.UserApprovals.Remove(userApproval);
            }

            foreach (var approvalId in model.ApprvoalIds)
            {
                var userApproval = new UserApproval
                {
                    ApprovalId = approvalId,
                    UserId = model.UserId
                };

                _context.UserApprovals.Add(userApproval.FillCommonProperties());
            }

            await _context.SaveChangesAsync();
        }
    }
}
