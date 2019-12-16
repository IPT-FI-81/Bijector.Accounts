using System.Threading.Tasks;
using Bijector.Accounts.Messages.Events;
using Bijector.Accounts.Models;
using Bijector.Accounts.Repositories;
using Bijector.Infrastructure.Handlers;
using Bijector.Infrastructure.Types;

namespace Bijector.Accounts.Handlers.Events
{
    public class AddServiceHandler : IEventHandler<AddService>
    {
        private readonly IAccountStore accountStore;

        public AddServiceHandler(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task Handle(AddService command, IContext context)
        {
            var service = new Service
            {
                UserServiceId = command.UserServiceId,
                ServiceName = command.ServiceName,
                UserServiceName = command.UserServiceName,
                ServiceType = command.ServiceType
            };
            await accountStore.AddLinkedServiceAsync(context.UserId, service);
        }
    }
}