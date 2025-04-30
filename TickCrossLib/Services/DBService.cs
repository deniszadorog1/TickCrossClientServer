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
using TickCrossLib.EntityModels;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Security.Policy;
using TickCrossLib.Models;
using System.Windows.Forms;
using Microsoft.IdentityModel.Protocols.OpenIdConnect.Configuration;
using System.Runtime.Remoting.Metadata;
using TickCrossLib.Enums;

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

        public static Models.User GetUserById(int id)
        {
            return GetAllUsers().FirstOrDefault(x => x.Id == id);
        }

        public static void AddNewUser(string login, string password)
        {
            using (var system = new TickCross())
            {
                EntityModels.User user = new EntityModels.User();
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

        public static EntityModels.User GetDBUserByLogin(string login)
        {
            using (var system = new TickCross())
            {
                return system.User.FirstOrDefault(x => x.Login == login);
            }
        }

        public static Models.User GetUserModelByLogin(string login)
        {
            EntityModels.User user = GetDBUserByLogin(login);

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

        public static int? GetSignId(char sign)
        {
            using (var model = new TickCross())
            {
                var type = model.SignType
                    .AsEnumerable()
                    .FirstOrDefault(x => !string.IsNullOrEmpty(x.Type) && x.Type[0] == sign);

                return type?.Id;
            }
        }

        public static char? GetSignById(int id)
        {
            using (var model = new TickCross())
            {
                SignType type = model.SignType.Where(x => x.Id == id).FirstOrDefault();

                if (type is null) return null;
                return type.Type[0];
            }
        }

        public static void SetLoginStatusToUser(string login, bool status)
        {
            using (var model = new TickCross())
            {
                var user = model.User.Where(x => x.Login == login).FirstOrDefault();
                if (user is null) return;
                user.IsLogged = status;
            }
        }

        public static bool IsUserIsLogged(string login)
        {
            using (var model = new TickCross())
            {
                return model.User.Where(x => x.Login == login && !(x.IsLogged == null) && (bool)x.IsLogged).Any();
            }

        }

        public static bool IsUserIsLoggedById(int id)
        {
            using (var model = new TickCross())
            {
                return model.User.Where(x => x.Id == id && !(x.IsLogged == null) && (bool)x.IsLogged).Any();
            }
        }

        public static void AddTempGame(int gameId)
        {
            if (IsTempGameIsExist(gameId)) return;
            using (var model = new TickCross())
            {
                TempGame tempGame = new TempGame();
                tempGame.TempMoveX = null;
                tempGame.TempMoveY = null;

                tempGame.GameId = gameId;
                tempGame.StepperId = GetStartStepper(gameId);

                model.TempGame.Add(tempGame);

                model.SaveChanges();
            }
        }

        private static int GetStartStepper(int gameId)
        {
            List<char> signs = GetAllSigns();
            EntityModels.Game game = GetGameById(gameId);
            if (signs is null || game is null) return (int)game.FirstPlayerId;

            return GetSignById((int)game.FirstSignId) == signs.First() ?
                (int)game.FirstPlayerId : (int)game.SecondPlayerId;
        }

        private static EntityModels.Game GetGameById(int gameId)
        {
            using (var model = new TickCross())
            {
                return model.Game.Where(x => x.Id == gameId).FirstOrDefault();
            }
        }

        public static bool IsTempGameIsExist(int gameId)
        {
            using (var model = new TickCross())
            {
                return model.TempGame.Where(x => x.GameId == gameId).Any();
            }
        }

        public static bool AddGameRequest(string senderLogin, string receiverLogin,
            int enemySignId, int userSignId, int statusId)
        {
            using (var model = new TickCross())
            {
                Models.User sendUser = GetUserByLogin(senderLogin);
                Models.User recUser = GetUserByLogin(receiverLogin);
                if (senderLogin is null || recUser is null) return false;

                EntityModels.GameRequest req = new EntityModels.GameRequest();

                req.SenderId = sendUser.Id;
                req.ReciverId = recUser.Id;

                req.SenderSignId = enemySignId;
                req.ReciverSignId = userSignId;

                req.StatusId = statusId;

                model.GameRequest.Add(req);

                model.SaveChanges();

                return true;
            }
        }

        public static Models.GameRequest GetFirstUnwatchedUserRequest(string login)
        {
            using (var model = new TickCross())
            {
                foreach (var req in model.GameRequest)
                {
                    var sender = GetDbUserById((int)req.SenderId);
                    var receiver = GetDbUserById((int)req.ReciverId);

                    if (!(sender is null) && !(receiver is null) &&
                        (sender.Login == login || receiver.Login == login))
                    {
                        Models.User senderUser = GetUserById((int)req.SenderId);
                        Models.User receiverUser = GetUserById((int)req.ReciverId);

                        char? senderSign = GetSignById((int)req.SenderSignId);
                        char? receiverSign = GetSignById((int)req.ReciverSignId);

                        if (senderSign is null || receiverSign is null) return null;

                        return new Models.GameRequest(req.Id, senderUser, receiverUser, (char)senderSign, (char)receiverSign);
                    }
                }
            }

            return null;
        }

        public static void RemoveUserRequests(int userId)
        {
            using (var model = new TickCross())
            {
                List<EntityModels.GameRequest> toRemove = new List<EntityModels.GameRequest>();

                foreach (var req in model.GameRequest)
                {
                    EntityModels.User first = GetDbUserById((int)req.SenderId);
                    EntityModels.User second = GetDbUserById((int)req.ReciverId);

                    if (first.Id == userId || second.Id == userId) toRemove.Add(req);
                }

                model.GameRequest.RemoveRange(toRemove);
                model.SaveChanges();
            }
        }

        public static (int?, int?) GetMoveCordToMake(int gameId)
        {
            using (var model = new TickCross())
            {
                TempGame tempGame = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (tempGame is null) return (null, null);

                return (tempGame.TempMoveX, tempGame.TempMoveY);
            }
        }

        public static EntityModels.User GetDbUserById(int id)
        {
            using (var model = new TickCross())
            {
                return model.User.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public static void SetTempGameStatus(string newStatus, int gameId)
        {
            using (var model = new TickCross())
            {
                TempGame temp = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (temp is null) return;

                int? statusId = GetStatusIdByName(newStatus);
                if (statusId is null) return;

                temp.TypeId = statusId;
                model.SaveChangesAsync();
            }
        }

        public static string GetTempGameStatus(int gameId)
        {
            using (var model = new TickCross())
            {
                TempGame game = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (game is null) return null;

                return GetStatusById(game.TypeId);
            }
        }

        private static string GetStatusById(int? id)
        {
            if (id is null) return null;
            using (var model = new TickCross())
            {
                TempGameType type =
                    model.TempGameType.Where(x => x.Id == id).FirstOrDefault();

                if (type is null) return null;
                return type.Type;
            }
        }

        private static int? GetStatusIdByName(string status)
        {
            using (var model = new TickCross())
            {
                TempGameType type =
                    model.TempGameType.Where(x => x.Type == status).FirstOrDefault();

                if (type is null) return null;
                return type.Id;
            }
        }

        public static void SetTempGameMoveCord((int, int) cord, int gameId)
        {
            using (var model = new TickCross())
            {
                TempGame game = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (game is null) return;
                game.TempMoveX = cord.Item1;
                game.TempMoveY = cord.Item2;

                model.SaveChanges();
            }
        }

        public static void AddGame(Models.Game game)
        {
            using (var model = new TickCross())
            {
                EntityModels.Game toAdd = new EntityModels.Game();

                toAdd.FirstPlayerId = game.FirstPlayer.Id;
                toAdd.SecondPlayerId = game.SecondPlayer.Id;

                toAdd.FirstSignId = GetSignId(game.FirstPlayerSign);
                toAdd.SecondSignId = GetSignId(game.SecondPlayerSign);

                model.Game.Add(toAdd);

                model.SaveChanges();
            }
        }

        public static int? GetLastGameId()
        {
            using (var model = new TickCross())
            {
                EntityModels.Game game = model.Game.OrderByDescending(g => g.Id).FirstOrDefault();
                if (game is null) return null;

                return game.Id;
            }
        }

        public static int? GetRequestStatusId(string status)
        {
            using (var model = new TickCross())
            {
                EntityModels.RequestStatus stat = model.RequestStatus.Where(x => x.Type == status).FirstOrDefault();
                if (stat is null) return null;
                return stat.Id;
            }
        }

        public static string GetRequestStatusId(int id)
        {
            using (var model = new TickCross())
            {
                EntityModels.RequestStatus stat = model.RequestStatus.Where(x => x.Id == id).FirstOrDefault();
                if (stat is null) return null;
                return stat.Type;
            }
        }

        public static string GetRequestsStatus(int requestId)
        {
            using (var model = new TickCross())
            {
                EntityModels.GameRequest req = model.GameRequest.Where(x => x.Id == requestId).FirstOrDefault();
                if (req is null) return null;

                return GetRequestStatusId((int)req.StatusId);

            }
        }

        public static void SetRequestStatus(int reqId, int statusId)
        {
            using (var model = new TickCross())
            {
                EntityModels.GameRequest req = model.GameRequest.Where(x => x.Id == reqId).FirstOrDefault();
                if (req is null) return;

                req.StatusId = statusId;
                model.SaveChanges();
            }
        }

        public static void SetStepperInTempGame(int gameId, int stepperId)
        {
            using (var model = new TickCross())
            {
                TempGame game = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (game is null) return;
                game.StepperId = stepperId;

                model.SaveChanges();
            }
        }

        public static int? GetStepperIdInTempGame(int gameId)
        {
            using (var model = new TickCross())
            {
                TempGame game = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (game is null) return null;

                return game.StepperId;
            }
        }

        public static int GetWinsAmount(int userId)
        {
            using (var model = new TickCross())
            {
                return model.Game.Where(x => x.WinnerId == userId).Count();
            }
        }

        public static int GetLosesAmount(int userId)
        {
            using (var model = new TickCross())
            {
                return model.Game.Where(x => x.WinnerId != userId &&
                (x.FirstPlayerId == userId || x.SecondPlayerId == userId)).Count();
            }
        }

        public static int GetGamesAmount(int userId)
        {
            using (var model = new TickCross())
            {
                return model.Game.Where(x => x.FirstPlayerId == userId || x.SecondPlayerId == userId).Count();
            }
        }

        public static void SetGameResult(int gameId, int? winnerId, bool? isDraw)
        {
            using (var model = new TickCross())
            {
                EntityModels.Game game = model.Game.Where(x => x.Id == gameId).FirstOrDefault();
                if (game is null) return;

                game.WinnerId = winnerId;
                game.IsDraw = isDraw;

                model.SaveChanges();
            }
        }

        public static void RemoveTempGame(int gameId)
        {
            using (var model = new TickCross())
            {
                model.TempGame.RemoveRange(model.TempGame.Where(x => x.GameId == gameId));
                model.SaveChanges();
            }
        }

        public static void RemoveClosedGames(int userId)
        {
            using(var model = new TickCross())
            {
                var gamesToDelete = model.Game
                    .Where(g => (g.FirstPlayerId == userId || g.SecondPlayerId == userId)
                             && (g.IsDraw == null && g.WinnerId == null))
                    .ToList();

                var gameIds = gamesToDelete.Select(g => g.Id).ToList();

                var tempGamesToDelete = model.TempGame
                    .Where(t => gameIds.Contains(t.GameId ?? 0))  
                    .ToList();

                var requests = model.GameRequest.Where(x => x.SenderId == userId || x.ReciverId == userId);

                model.GameRequest.RemoveRange(requests);

                model.TempGame.RemoveRange(tempGamesToDelete);

                model.Game.RemoveRange(gamesToDelete);

                model.SaveChanges();
            }
        }

        private static List<EntityModels.Game> GetGamesWhereUserIsPlaying(int userId)
        {
            using(var model = new TickCross())
            {
                return model.Game.Where(x => (x.FirstPlayerId == userId || x.SecondPlayerId == userId) && 
                (x.IsDraw == null && x.WinnerId == null)).ToList();
            }
        }

        public static void SetClosedStatusToGame(int gameId)
        {
            using(var model = new TickCross())
            {
                int? cancelTypeId =  GetCanceledGameStatusId();
                if (cancelTypeId is null) return;

               TempGame tempGame = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (tempGame is null) return;

                tempGame.TypeId = cancelTypeId;
                model.SaveChanges();
            }
        }

        public static int? GetCanceledGameStatusId()
        {
            using(var model = new TickCross())
            {
                TempGameType type = model.TempGameType.Where(x => x.Type == GameEnded.Canceled.ToString()).FirstOrDefault();
                if (type is null) return null;
                return type.Id;
            }
        }

        public static bool IsTempGameIsBeenCanceled(int gameId)
        {
            using(var model = new TickCross())
            {
                TempGame game = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (game is null) return false;

                return game.TypeId == GetCanceledGameStatusId(); 
            }
        }
    }
}
