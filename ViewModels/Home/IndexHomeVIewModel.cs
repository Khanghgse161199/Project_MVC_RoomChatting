using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ChatRoom;
using ViewModels.Profile;
using DataServices.Entities;
using ViewModels.Message;

namespace ViewModels.Home
{
    public class IndexHomeVIewModel
    {
        public ProfileViewModel ProfileViewModel { get; set; }
        public CreateChatRoomViewModel? CreateChatRoomViewModel { get; set; }
        public List<DataServices.Entities.ChatRoom> chatRooms { get; set; }
        public ChatDetailViewModel ChatDetailViewModel { get; set; }
        public CreateMessageViewModel CreateMessageViewModel { get; set; }
        public JoinChatViewModel JoinChatViewModel { get; set; }
    }
}
