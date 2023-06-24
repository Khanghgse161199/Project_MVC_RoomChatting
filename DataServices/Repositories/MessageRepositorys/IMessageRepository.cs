using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.MessageRepositorys
{
    public interface IMessageRepository : IRepository<Message>
    {
        
    }
}
