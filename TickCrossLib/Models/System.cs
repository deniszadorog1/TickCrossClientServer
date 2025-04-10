using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickCrossLib.Models
{
    public class System
    {
        public List<User> Users { get; set; }


        public System()
        {
            Users = new List<User>();
        }

    }
}
