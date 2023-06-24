using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Users
{
    public class UserRepository : Repository<User> , IUserRepository
    {
        private readonly RoomChattingContext _context;
        public UserRepository(RoomChattingContext context):base(context)
        {
            _context = context;
        }

        public void Udpate(User user)
        {
            _context.Update(user);
        }
    }
}
