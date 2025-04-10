using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickCrossLib.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User()
        {
            Id = -1;
            Login = string.Empty;
            Password = string.Empty;
        }

        public User(string login, string password, int id)
        {
            Id = id;
            Login = login;
            Password = password;
        }

        public string GetLogin()
        {
            return Login;
        }

       
    }
}
