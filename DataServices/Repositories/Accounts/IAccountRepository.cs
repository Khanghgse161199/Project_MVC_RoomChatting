using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Accounts
{
    public interface IAccountRepository : IRepository<Account>
    {
        void Udpate(Account account);
    }
}
