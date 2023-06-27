using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServices.Entities;

namespace DataServices.Repositories.ChatRoomUserMapping
{
    public class RoomUserMappingRepository : Repository<RoomUserMapping>, IRooUserMappingRepository
    {
        private readonly RoomChattingContext _context;
        public RoomUserMappingRepository(RoomChattingContext context) : base(context)
        {
            _context = context;
        }
        public void Udpate(Entities.RoomUserMapping RoomUserMapping)
        {
            _context.Update(RoomUserMapping);
        }
    }
}
