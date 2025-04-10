using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TickCrossLib.EntityModels;

namespace TickCrossLib.Services
{
    internal static class DBService
    {
        public static List<Models.User> GetAllUsers()
        {
            List<Models.User> result = new List<Models.User>();
            using(var system = new TickCross())
            {
                foreach(var user in system.User)
                {
                    result.Add(new Models.User(user.Login, user.Password,user.Id));
                }
            }
            return result;
        }

        public static Models.User GetUserByLogin(string login)
        {
            return  GetAllUsers().FirstOrDefault(x => x.GetLogin() == login);
        }

        public static void AddNewUser(string login, string password)
        {
            using(var system = new TickCross())
            {
                User user = new User();
                user.Login = login;
                user.Password = password;

                system.User.Add(user);

                system.SaveChanges();
            }
        }




    }
}
