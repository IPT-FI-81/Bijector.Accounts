using System.Threading.Tasks;
using IdentityServer4.Services;

namespace Bijector.Accounts.Services
{
    public class CORSPolicyService : ICorsPolicyService
    {
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            //because course-work deadline ;)
            return true;
        }
    }
}