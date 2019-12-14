using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Bijector.Accounts.Models;
using Bijector.Accounts.Messages.Queries;
using Bijector.Accounts.Repositories;
using Bijector.Infrastructure.Handlers;
using Bijector.Infrastructure.Types;

namespace Bijector.Accounts.Handlers.Queries
{
    public class RegisterRequestHandler : IQueryHandler<RegisterRequest, Account>
    {
        private readonly IAccountStore accountStore;            

        public RegisterRequestHandler(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<Account> Handle(RegisterRequest query, IContext context)
        {
            //logger.LogInformation("Handle register request");
            
            if(await accountStore.CreateAsync(query.FirstName, query.SecondName, query.Login, query.Password))
            {
                //logger.LogInformation("Successfully register");
                return await accountStore.GetAsync(query.Login);
            }
            //logger.LogError("Error in register");
            return null;
        }
    }
}