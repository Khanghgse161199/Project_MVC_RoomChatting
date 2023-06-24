using DataService.HashService;
using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Auth;
using ViewModels.Token;

namespace Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResultViewModel> LoginAsync(string email, string password);
        Task<CheckTokenResultViewModel> checkTokenAsyc(string token);
    }
    public class AuthService: IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHashService _hashService;
        private string key = "testPassword";
        public AuthService(IUnitOfWork uow, IHashService hashService)
        {
            _uow = uow;
            _hashService = hashService; 
        }

        public async Task<LoginResultViewModel> LoginAsync(string email, string password)
        {
            var currentAcc = await _uow.Accounts.FirstOfDefaultAsync(p => p.Email == email && p.Password == _hashService.SHA256(password + key));
            if (currentAcc != null)
            {
                var tokenDb = await _uow.Tokens.FirstOfDefaultAsync(p => p.AccId == currentAcc.Id && p.IsActive);
                string newTokenAccess = Guid.NewGuid().ToString();
                if(tokenDb != null)
                {
                    tokenDb.AccessToken = newTokenAccess;
                    tokenDb.DateTime = DateTime.Now;
                    _uow.Tokens.Udpate(tokenDb);
                    await _uow.SaveAsync();
                    return new LoginResultViewModel()
                    {
                        AccessToken = newTokenAccess
                    };
                }
                else
                {
                    Token newToken = new Token() { 
                        Id = Guid.NewGuid().ToString(),
                        AccessToken = newTokenAccess,
                        AccId = currentAcc.Id,
                        DateTime = DateTime.Now,
                        IsActive = true
                    };
                    await _uow.Tokens.AddAsync(newToken);
                    await _uow.SaveAsync();
                    return new LoginResultViewModel()
                    {
                        AccessToken = newTokenAccess
                    };
                }
            }
            else return null;
        }

        public async Task<CheckTokenResultViewModel> checkTokenAsyc(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var currentToken = await _uow.Tokens.FirstOfDefaultAsync(p => p.AccessToken == token && ((DateTime.Now.Day - p.DateTime.Day) <= 2) && p.IsActive, "Acc");
                if (currentToken != null)
                {
                    CheckTokenResultViewModel checkTokenResult = new CheckTokenResultViewModel() { 
                        accId = currentToken.AccId,
                    };
                    return checkTokenResult;
                }
                return null;
            }
            else return null;
        }
    }
}
