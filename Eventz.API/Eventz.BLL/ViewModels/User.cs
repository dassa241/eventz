using System;
using System.Collections.Generic;
using System.Text;

namespace Eventz.BLL.ViewModels
{
    public class User
    {
        public int UserIdx { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
