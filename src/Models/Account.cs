using System;
using System.Collections.Generic;
using Bijector.Infrastructure.Types;

namespace Bijector.Accounts.Models
{
    public class Account : IIdentifiable
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<Service> LinkedService { get; set; }
    }
}