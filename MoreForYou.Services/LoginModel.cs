using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services
{
    public class LoginModel
    {
        public long UserNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int LanguageId { get; set; }
        public bool RememberMe { get; set; }
    }
}
