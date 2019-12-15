using System;
using System.Threading.Tasks;
using Bijector.Accounts.Models;

namespace Bijector.Accounts.Repositories
{
    public interface IAccountStore
    {
        Task<bool> ValidateCredentialsAsync(string login, string password);

        Task<bool> ChangePasswordAsync(int id, string lastPassword, string newPassword);        

        Task<bool> CreateAsync(string firstName, string lastName, string login, string password);

        Task<bool> DeleteAsync(int id);

        Task<Account> GetAsync(string login);

        Task<Account> GetAsync(int id);

        Task<bool> IsExistsAsync(string login);

        Task<bool> IsExistsAsync(int id);

        Task<bool> UpdateAsync(int id, Account newAccountData);

        Task<bool> AddLinkedServiceAsync(int id, Service service);

        Task<bool> RemoveLinkedServiceAsync(int id, Service service);
    }
}