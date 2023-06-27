using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ChatRoom
{
    public class ChatDetailViewModel
    {
       public ChatRoomViewModel ChatRoom { get; set; }
        public List<MessageViewModel> messages { get; set; }
    }
    public class MessageViewModel
    {
        public string Id { get; set; }
        public string Messaget { get; set; }
        public string? ImgUrl { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }  
        public bool IsMessageSystem { get; set; }
        public DateTime DateTime { get; set; }

    }
    public class ChatRoomViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Admin { get; set; }
        public bool IsPrivate { get; set; }
    }
}
