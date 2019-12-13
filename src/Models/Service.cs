using System;

namespace Bijector.Accounts.Models
{
    public class Service
    {
        public Guid UserServiceId { get; set; }

        public string ServiceName { get; set; }

        public string UserServiceName { get; set; }

        public string ServiceType { get; set; }
    }
}