using System;
using System.Collections.Generic;
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
            var login = context.Subject.Identity.Name;            
            if (await accountStore.IsExistsAsync(login))
            {
                var account = await accountStore.GetAsync(login);
                //context.IssuedClaims = new List<Claim>{new Claim("iden", account.Id.ToString())};
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}