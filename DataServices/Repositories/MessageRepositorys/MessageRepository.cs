using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.MessageRepositorys
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly RoomChattingContext _context;
        public MessageRepository(RoomChattingContext context):base(context) 
        {
            _context = context;
        }
    }
}
