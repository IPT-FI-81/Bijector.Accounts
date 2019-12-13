using System;
using System.Threading.Tasks;
using Bijector.Accounts.Models;

namespace Bijector.Accounts.Repositories
{
    public interface IAccountStore
    {
        Task<bool> ValidateCredentialsAsync(string login, string password);

        Task<bool> ChangePasswordAsync(Guid id, string lastPassword, string newPassword);        

        Task<bool> CreateAsync(string firstName, string lastName, string login, string password);

        Task<bool> DeleteAsync(Guid id);

        Task<Account> GetAsync(string login);

        Task<Account> GetAsync(Guid id);

        Task<bool> IsExistsAsync(string login);

        Task<bool> IsExistsAsync(Guid id);

        Task<bool> UpdateAsync(Guid id, Account newAccountData);

        Task<bool> AddLinkedServiceAsync(Guid id, Service service);

        Task<bool> RemoveLinkedServiceAsync(Guid id, Service service);
    }
}