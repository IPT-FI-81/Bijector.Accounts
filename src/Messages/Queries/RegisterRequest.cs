using Bijector.Accounts.Models;
using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Accounts.Messages.Queries
{
    public class RegisterRequest : IQuery<Account>
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}