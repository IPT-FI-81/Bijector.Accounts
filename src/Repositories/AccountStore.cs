using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bijector.Accounts.Models;
using Bijector.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Bijector.Accounts.Repositories
{
    public class AccountStore : IAccountStore
    {
        private readonly IRepository<Account> accountRepository;

        private readonly IPasswordHasher<Account> passwordHasher;

        public AccountStore(IRepository<Account> accountRepository, IPasswordHasher<Account> passwordHasher)
        {
            this.accountRepository = accountRepository;
            this.passwordHasher = passwordHasher;
        }

        public async Task<bool> AddLinkedServiceAsync(Guid id, Service service)
        {   
            if(await IsExistsAsync(id))
            {
                var account = await accountRepository.GetByIdAsync(id);
                service.UserServiceId = Guid.NewGuid();                
                account.LinkedService.Add(service);
                await accountRepository.UpdateAsync(id, account);
                return true;
            }
            return false;         
        }

        public async Task<bool> ChangePasswordAsync(Guid id, string oldPassword, string newPassword)
        {
            if(await IsExistsAsync(id))
            {
                var account = await accountRepository.GetByIdAsync(id);
                if(ValidatePassword(account, oldPassword))
                {
                    account.PasswordHash = passwordHasher.HashPassword(account, newPassword);
                    await accountRepository.UpdateAsync(id, account);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CreateAsync(string firstName, string lastName, string login, string password)
        {          
            if(!await IsExistsAsync(login))
            {
                var account = new Account
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Login = login
                };
                account.PasswordHash = passwordHasher.HashPassword(account, password);
                account.LinkedService = new List<Service>();
                await accountRepository.AddAsync(account);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if(await IsExistsAsync(id))
            {                
                await accountRepository.RemoveAsync(id);
                return true;
            }
            return false;
        }

        public async Task<Account> GetAsync(string login)
        {
            return await accountRepository.FindAsync(acc => acc.Login == login);
        }

        public async Task<Account> GetAsync(Guid id)
        {
            return await accountRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsExistsAsync(string login)
        {            
            return await accountRepository.IsExistsAsync(acc => acc.Login == login);
        }

        public async Task<bool> IsExistsAsync(Guid id)
        {
            return await accountRepository.IsExistsAsync(id);
        }

        public async Task<bool> RemoveLinkedServiceAsync(Guid id, Service service)
        {
            if(await accountRepository.IsExistsAsync(id))
            {
                var account = await accountRepository.GetByIdAsync(id);
                account.LinkedService.Remove(service);
                await accountRepository.UpdateAsync(id, account);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Guid id, Account newAccountData)
        {
            if(await IsExistsAsync(id))
            {
                var account = await accountRepository.GetByIdAsync(id);
                
                if(!await IsExistsAsync(newAccountData.Login))
                    account.Login = newAccountData.Login;

                account.LastName = newAccountData.LastName;
                account.FirstName = newAccountData.FirstName;
                await accountRepository.UpdateAsync(id, account);
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateCredentialsAsync(string login, string password)
        {
            if(await IsExistsAsync(login))
            {
                var account = await GetAsync(login);
                return ValidatePassword(account, password);
            }
            return false;
        }

        private bool ValidatePassword(Account account, string password)
        {
            var passwordHash = passwordHasher.HashPassword(account, password);
            if(passwordHash == account.PasswordHash)
            {
                return true;
            }
            return false;
        }
    }
}