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
using System.Data;
using TickCrossLib.EntityModel;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Security.Policy;
using TickCrossLib.Models;
using System.Windows.Forms;

namespace TickCrossLib.Services
{
    public static class DBService
    {
        public static List<Models.User> GetAllUsers()
        {
            List<Models.User> result = new List<Models.User>();
            using (var system = new TickCross())
            {
                foreach (var user in system.User)
                {
                    result.Add(new Models.User(user.Login, user.Password, user.Id));
                }
            }
            return result;
        }

        public static List<Models.User> GetUserEnemies(string login)
        {
            return GetAllUsers().Where(x => x.Login != login).ToList();
        }

        public static Models.User GetUserByLogin(string login)
        {
            return GetAllUsers().FirstOrDefault(x => x.GetLogin() == login);
        }

        public static Models.User GetLoggedUser(string login, string password)
        {
            return GetAllUsers().FirstOrDefault(x => x.Login == login && x.Password == password);
        }

        public static void AddNewUser(string login, string password)
        {
            using (var system = new TickCross())
            {
                EntityModel.User user = new EntityModel.User();
                user.Login = login;
                user.Password = password;

                system.User.Add(user);

                system.SaveChanges();
            }
        }

        public static void AddFriend(string playerLogin, string friendLogin)
        {
            using (var system = new TickCross())
            {
                Models.User player = GetUserByLogin(playerLogin);
                Models.User friend = GetUserByLogin(friendLogin);

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

        public static bool RemoveFriend(string playerLogin, string friendLogin)
        {
            using (var system = new TickCross())
            {
                Models.User player = GetUserByLogin(playerLogin);
                Models.User friend = GetUserByLogin(friendLogin);

                var playerFriend = system.UserFriend
                    .FirstOrDefault(x => x.UserId == player.Id && x.FriendId == friend.Id);

                var friendPlayer = system.UserFriend
                    .FirstOrDefault(x => x.UserId == friend.Id && x.FriendId == player.Id);

                bool anyRemoved = false;

                if (playerFriend != null)
                {
                    system.UserFriend.Remove(playerFriend);
                    anyRemoved = true;
                }

                if (friendPlayer != null)
                {
                    system.UserFriend.Remove(friendPlayer);
                    anyRemoved = true;
                }

                if (anyRemoved)
                {
                    return system.SaveChanges() > 0;
                }

                return false;
            }
        }

        public static List<Models.User> GetAllUserFriends(string login)
        {
            List<Models.User> result = new List<Models.User>();
            using (var system = new TickCross())
            {
                foreach (var friend in system.UserFriend)
                {
                    if (friend.User.Login == login)
                    {
                        result.Add(GetUserModelByLogin(friend.User1.Login));
                    }
                }
            }
            return result;
        }

        public static List<Models.User> GetUsersThatNotFriend(string login)
        {
            List<Models.User> friends = GetAllUserFriends(login);
            List<Models.User> result = new List<Models.User>();

            using (var system = new TickCross())
            {
                foreach (var user in system.User)
                {
                    if (!friends.Where(x => x.Login == user.Login).Any())
                    {
                        result.Add(GetUserModelByLogin(user.Login));
                    }
                }
            }
            return result;
        }


        public static bool IsUserLoginIsExist(string login)
        {
            using (var system = new TickCross())
            {
                return system.User.Any(x => x.Login == login);
            }

        }

        public static bool ChangeUserParams(string oldLogin, string newLogin, string newPassword)
        {
            using (var system = new TickCross())
            {
                var user = system.User.FirstOrDefault(u => u.Login == oldLogin);
                if (user is null) return false;

                if (!string.IsNullOrEmpty(newLogin)) user.Login = newLogin;
                if (!string.IsNullOrEmpty(newPassword)) user.Password = newPassword;

                return system.SaveChanges() > 0;
            }
        }

        public static EntityModel.User GetDBUserByLogin(string login)
        {
            using (var system = new TickCross())
            {
                return system.User.FirstOrDefault(x => x.Login == login);
            }
        }

        public static Models.User GetUserModelByLogin(string login)
        {
            EntityModel.User user = GetDBUserByLogin(login);

            return new Models.User()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password
            };
        }

        public static List<char> GetAllSigns()
        {
            List<char> result = new List<char>();
            using (var system = new TickCross())
            {
                foreach (var type in system.SignType)
                {
                    result.Add(type.Type.ToString().First());
                }
            }

            return result;
        }

        public static Models.User GetFirstUnwatchedUserRequest(string reciverLogin)
        {
            Models.User result = null;


            return result;
        }

        public static (int?, int?) GetMoveToMake(int stepperiId)
        {
            (int?, int?) resCord = (null, null);


            //Get cord from temp game(db) table


            return resCord;
        }

    }
}
