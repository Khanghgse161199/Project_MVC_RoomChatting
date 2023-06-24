using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServices.Entities;

namespace DataServices.Repositories.ChatRoom
{
    public interface IChatRoomRepository: IRepository<DataServices.Entities.ChatRoom>
    {
        void Udpate(DataServices.Entities.ChatRoom chatRoom);
    }
}
