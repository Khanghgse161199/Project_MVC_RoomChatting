using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Users
{
    public interface IUserRepository : IRepository<User>
    {
        void Udpate(User user);
    }
}
