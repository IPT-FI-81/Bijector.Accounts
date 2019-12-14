using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Accounts.Messages.Queries
{
    public class LoginRequest : IQuery<bool>
    {
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}