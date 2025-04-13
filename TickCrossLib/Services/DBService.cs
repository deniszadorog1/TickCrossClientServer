using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

        public static void AddFriend(Models.User player, Models.User friend)
        {
            using(var system = new TickCross())
            {
/*                UserFriend addFriend = new UserFriend();
                addFriend.FriendId = friend.Id;
                addFriend.UserId = player.Id;

                system.UserFriend.Add(addFriend);*/

                system.UserFriend.Add(GetUserFriendToAdd(player.Id, friend.Id));
                system.UserFriend.Add(GetUserFriendToAdd(friend.Id, player.Id));

                system.SaveChanges();
            }
        }

        private static UserFriend GetUserFriendToAdd(int userId, int friendId)
        {
            UserFriend userFriend = new UserFriend();

            userFriend.UserId = userId;
            userFriend.FriendId = friendId;

            return userFriend;
        }

        public static void RemoveFriend(Models.User player, Models.User friend)
        {
            using (var system = new TickCross())
            {
                //Remove from player friends
                system.UserFriend.Remove(system.UserFriend.Where(x => x.UserId == player.Id && x.FriendId == friend.Id).FirstOrDefault());

                //Remove from friend friends
                system.UserFriend.Remove(system.UserFriend.Where(x => x.UserId == friend.Id && x.FriendId == player.Id).FirstOrDefault());
            }
        }

        public static bool IsUserLoginIsExist(string login)
        {
            using(var system = new TickCross())
            {
                return system.User.Any(x => x.Login == login);
            }
        }

        public static void ChangeUserParams(string oldLogin, string newLogin, string newPassword)
        {
            using (var system = new TickCross())
            {
                User user = GetDBUserByLogin(oldLogin);
                if (user is null) return;

                if (!string.IsNullOrEmpty(newLogin)) user.Login = newLogin;
                if (!string.IsNullOrEmpty(newPassword)) user.Password = newPassword;

                system.SaveChanges();
            }
        }

        public static User GetDBUserByLogin(string login)
        {
            using(var system = new TickCross())
            {
                return system.User.FirstOrDefault(x => x.Login == login);
            }
        }

    }
}
