using DataService.HashService;
using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AccountServices
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(string email, string password, string AccountId);
    }
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHashService _hashService;
        private string key = "testPassword";
        public AccountService(IUnitOfWork unitOfWork, IHashService hashService)
        {
            _uow = unitOfWork;
            _hashService = hashService;
        }
        public async Task<bool> CreateAccountAsync(string email, string password, string AccountId)
        {
            var checkExist = await _uow.Accounts.FirstOfDefaultAsync(p => p.Email == email);
            if (checkExist == null)
            {
                var newAccount = new Account()
                {
                    Id = AccountId,
                    Email = email,
                    Password = _hashService.SHA256(password + key),
                    IsActive = true,
                };

                await _uow.Accounts.AddAsync(newAccount);
                await _uow.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
