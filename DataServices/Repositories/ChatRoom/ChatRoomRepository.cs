using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServices.Entities;

namespace DataServices.Repositories.ChatRoom
{
    public class ChatRoomRepository : Repository<DataServices.Entities.ChatRoom>, IChatRoomRepository
    {
        private readonly RoomChattingContext _context;
        public ChatRoomRepository(RoomChattingContext context) : base(context)
        {
            _context = context;
        }
        public void Udpate(Entities.ChatRoom chatRoom)
        {
            _context.Update(chatRoom);
        }
    }
}
