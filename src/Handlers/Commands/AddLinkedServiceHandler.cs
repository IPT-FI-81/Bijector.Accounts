using System;
using System.Threading.Tasks;
using Bijector.Accounts.Messages.Commands;
using Bijector.Accounts.Models;
using Bijector.Accounts.Repositories;
using Bijector.Infrastructure.Handlers;
using Bijector.Infrastructure.Types;

namespace Bijector.Accounts.Handlers.Commands
{
    public class AddLinkedServiceHandler : ICommandHandler<AddLinkedService>
    {
        private readonly IAccountStore accountStore;

        public AddLinkedServiceHandler(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task Handle(AddLinkedService command, IContext context)
        {            
            var service = new Service
            {                
                ServiceName = command.ServiceName,
                ServiceType = command.ServiceType,
                UserServiceName = command.UserServiceName
            };
            await accountStore.AddLinkedServiceAsync(context.UserId, service);
        }
    }
}