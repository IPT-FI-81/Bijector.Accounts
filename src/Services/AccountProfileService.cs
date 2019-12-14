using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bijector.Accounts.Repositories;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Bijector.Accounts.Services
{
    public class AccountProfileService : IProfileService
    {
        private readonly IAccountStore accountStore;

        public AccountProfileService(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = Guid.Parse(context.Subject.GetSubjectId());
            if (context.RequestedClaimTypes.Any() && await accountStore.IsExistsAsync(id))
            {
                var account = await accountStore.GetAsync(context.Subject.GetSubjectId());
                //context.AddRequestedClaims(new Claim("sub", context.Subject.GetSubjectId()));
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}