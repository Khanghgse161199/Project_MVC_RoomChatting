using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Auth
{
    public class SignUpViewModel
    {
        [Required]
        public string fullname {  get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }
        [Required]
        public DateTime birthDay { get; set; }
        [Required]
        public IFormFile file { get ; set; }

    }
}
