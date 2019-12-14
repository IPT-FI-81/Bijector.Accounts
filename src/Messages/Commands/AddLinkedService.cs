using System;
using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Accounts.Messages.Commands
{
    public class AddLinkedService : ICommand
    {
        public Guid UserServiceId { get; set; }

        public string ServiceName { get; set; }

        public string UserServiceName { get; set; }

        public string ServiceType { get; set; }
    }
}