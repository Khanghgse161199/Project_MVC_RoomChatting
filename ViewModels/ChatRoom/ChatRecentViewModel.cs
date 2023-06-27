using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ChatRoom
{
    public class ChatRecentViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int CountNotify { get; set; } 
        public string LastMessage { get; set; } = null!;
        public string SenderLastMessage { get; set; } = null!;
        public DateTime TimeSend { get; set; }
    }
}
