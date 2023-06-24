using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string AccId { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }

        public DateTime birthday { get; set; }
        public string ImgUrl { get; set; }
    }
}
