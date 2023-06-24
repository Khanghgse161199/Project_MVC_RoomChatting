using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.ChatRoomUserMapping
{
    public interface IRooUserMappingRepository : IRepository<DataServices.Entities.RoomUserMapping>
    {
        void Udpate(DataServices.Entities.ChatRoom RoomUserMapping);
    }
}
