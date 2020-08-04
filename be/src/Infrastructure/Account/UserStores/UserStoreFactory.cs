using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foundation.Account.UserStores
{
    public class UserStoreFactory
    {
        public IUserStore<TUser> CreateUserStore<TUser>(DbContext context) where TUser : IdentityUser, new()
        {
            return new UserStore<TUser>(context);
        }
    }
}
