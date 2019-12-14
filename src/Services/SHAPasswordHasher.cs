using Bijector.Accounts.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Bijector.Accounts.Services
{
    public class SHAPasswordHasher : IPasswordHasher<Account>
    {
        public string HashPassword(Account user, string password)
        {
            return password.Sha256();
        }

        public PasswordVerificationResult VerifyHashedPassword(Account user, string hashedPassword, string providedPassword)
        {
            if(HashPassword(user, providedPassword) == hashedPassword)
            {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }
}