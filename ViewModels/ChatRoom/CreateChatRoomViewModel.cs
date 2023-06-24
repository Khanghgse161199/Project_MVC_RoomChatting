using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ChatRoom
{
    public class CreateChatRoomViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        [Required]
        public string? serectKey { get; set; }   
    }
}
