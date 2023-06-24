using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Accounts
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly RoomChattingContext _context;
        public AccountRepository (RoomChattingContext context):base(context)
        {
            _context = context;
        }
        public void Udpate(Account account)
        {
            _context.Update(account);
        }
    }
}
