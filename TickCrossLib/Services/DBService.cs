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
using TickCrossLib.Models.NonePlayable;
using System.Reflection;
using System.Data.OleDb;
using System.Diagnostics.Contracts;

namespace TickCrossLib.Services
{
    public static class DBService
    {
        public static List<Models.User> GetAllUsers()
        {
            List<Models.User> result = new List<Models.User>();
            using (var model = new TickCross())
            {
                foreach (var user in model.User)
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

        public static void SetLoginStatusToUser(string login, UserStat status)
        {
            using (var model = new TickCross())
            {
                var user = model.User.Where(x => x.Login == login).FirstOrDefault();
                if (user is null) return;
                user.StatusId = GetUserStatusId(status.ToString());
            }
        }

        public static bool IsUserIsOnline(string login)
        {
            using (var model = new TickCross())
            {
                return model.User.Where(x => x.Login == login && !(x.StatusId == null) && IsUserStatusIsOnline((int)x.StatusId)).Any();
            }

        }

        public static bool IsUserIsLoggedById(int id)
        {
            using (var model = new TickCross())
            {
                return model.User.Where(x => x.Id == id && !(x.StatusId == null) && IsUserStatusIsOnline((int)x.StatusId)).Any();
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
                EntityModels.GameRequest req = model.GameRequest.Where(x => x.StatusId == requestId).FirstOrDefault();
                if (req is null) return null;

                return GetRequestStatusId((int)req.StatusId);
            }
        }

        public static Enums.RequestStatus GetGameReqStatusById(int id)
        {
            string status = GetRequestsStatus(id);

            for (int i = (int)Enums.RequestStatus.Accepted; i <= (int)Enums.RequestStatus.InProgress; i++)
            {
                if (((Enums.RequestStatus)i).ToString() == status.ToString())
                {
                    return ((Enums.RequestStatus)i);
                }
            }
            return Enums.RequestStatus.InProgress;
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
            using (var model = new TickCross())
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
            using (var model = new TickCross())
            {
                return model.Game.Where(x => (x.FirstPlayerId == userId || x.SecondPlayerId == userId) &&
                (x.IsDraw == null && x.WinnerId == null)).ToList();
            }
        }

        public static void SetClosedStatusToGame(int gameId)
        {
            using (var model = new TickCross())
            {
                int? cancelTypeId = GetCanceledGameStatusId();
                if (cancelTypeId is null) return;

                TempGame tempGame = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (tempGame is null) return;

                tempGame.TypeId = cancelTypeId;
                model.SaveChanges();
            }
        }

        public static int? GetCanceledGameStatusId()
        {
            using (var model = new TickCross())
            {
                TempGameType type = model.TempGameType.Where(x => x.Type == GameEnded.Canceled.ToString()).FirstOrDefault();
                if (type is null) return null;
                return type.Id;
            }
        }

        public static bool IsTempGameIsBeenCanceled(int gameId)
        {
            using (var model = new TickCross())
            {
                TempGame game = model.TempGame.Where(x => x.GameId == gameId).FirstOrDefault();
                if (game is null) return false;

                return game.TypeId == GetCanceledGameStatusId();
            }
        }

        public static void SetUserLoginStatus(int userId, UserStat status)
        {
            using (var model = new TickCross())
            {
                EntityModels.User user = model.User.Where(x => x.Id == userId).FirstOrDefault();
                if (user is null) return;

                user.StatusId = GetUserStatusId(status.ToString());

                model.SaveChanges();
            }
        }

        public static bool IsUserLogged(int userId)
        {
            using (var model = new TickCross())
            {
                EntityModels.User user = model.User.Where(x => x.Id == userId).FirstOrDefault();
                return user.StatusId is null || !(IsUserStatusIsOnline((int)user.StatusId)) ? false : true;
            }
        }

        public static List<EntityModels.GameRequest> GetAllGotUserRequests(int userId)
        {
            using (var model = new TickCross())
            {
                return model.GameRequest.Where(x => x.ReciverId == userId).ToList();
            }
        }

        public static List<EntityModels.GameRequest> GetGameRequestSentByUser(int userId)
        {
            using (var model = new TickCross())
            {
                return model.GameRequest.Where(x => x.SenderId == userId).ToList();
            }
        }

        public static string GetUserLoginById(int userId)
        {
            using (var model = new TickCross())
            {
                EntityModels.User user = model.User.Where(x => x.Id == userId).FirstOrDefault();
                return user is null ? null : user.Login;
            }
        }

        public static void AddFriendOffer(int senderId, int receiverId)
        {
            using (var model = new TickCross())
            {
                FriendOffer offer = new FriendOffer();

                offer.SenderId = senderId;
                offer.ReciverId = receiverId;
                offer.StatusId = GetFriendRequestStatusId(FriendRequestStatus.Sent);

                model.FriendOffer.Add(offer);

                model.SaveChanges();
            }
        }

        public static void ChangeStatusForFriendRequest(int senderId, int receiverId, FriendRequestStatus newStatus)
        {
            int friendReqId = GetFriendRequestBySenderReceiverIds(senderId, receiverId);
            using (var model = new TickCross())
            {
                model.FriendOffer.Where(x => x.Id == friendReqId).First().StatusId = GetFriendRequestStatusId(newStatus);
                model.SaveChanges();
            }
        }

        public static void RemoveFriendRequest(int senderId, int receiverId)
        {
            using (var model = new TickCross())
            {
                model.FriendOffer.Remove(model.FriendOffer.Where(x => x.SenderId == senderId && x.ReciverId == receiverId).First());
                model.SaveChanges();
            }
        }

        public static List<FriendRequestModel> GetFriendReqsSentByUser(int userId)
        {
            using (var model = new TickCross())
            {
                return GetFriendReqModels(model.FriendOffer.Where(x => x.SenderId == userId).ToList());
            }
        }

        public static List<FriendRequestModel> GetFriendReqsSentToUser(int userId)
        {
            using (var model = new TickCross())
            {
                return GetFriendReqModels(model.FriendOffer.Where(x => x.ReciverId == userId).ToList());
            }
        }

        public static List<string> GetFriendLogins(int userId)
        {
            List<string> res = new List<string>();
            using (var model = new TickCross())
            {
                List<EntityModels.UserFriend> userFriends = model.UserFriend.Where(x => x.UserId == userId).ToList();

                foreach (var userFriend in userFriends)
                {
                    res.Add(GetUserLoginById((int)userFriend.FriendId));
                }
                return res;
            }
        }

        public static List<string> GetUserLoginsToAddInFriends(int userId)
        {
            //That not friends 
            //That not requested
            //not userId

            List<string> res = new List<string>();
            using (var model = new TickCross())
            {
                List<int> friends = new List<int>();
                foreach (var friend in model.UserFriend)
                {
                    if (friend.FriendId == userId && friend.UserId != userId)
                    {
                        friends.Add((int)friend.UserId);
                    }
                }

                List<int> notFriendIds = new List<int>();

                foreach (var user in model.User)
                {
                    if (user.Id != userId && !friends.Contains(user.Id))
                    {
                        notFriendIds.Add(user.Id);
                    }
                }

                foreach (var offer in model.FriendOffer)
                {
                    if (notFriendIds.Contains((int)offer.SenderId) || notFriendIds.Contains((int)offer.ReciverId))
                    {
                        notFriendIds.Remove((int)offer.SenderId);
                        notFriendIds.Remove((int)offer.ReciverId);
                    }
                }

                for (int i = 0; i < notFriendIds.Count; i++)
                {
                    res.Add(GetUserLoginById(notFriendIds[i]));
                }
            }
            return res;
        }

        public static void RemoveFriendOfferBySenderLogin(int userId, string senderLogin)
        {
            int senderId = GetUserByLogin(senderLogin).Id;

            using (var model = new TickCross())
            {
                model.FriendOffer.Remove(model.FriendOffer.Where(
                    x => x.SenderId == senderId && x.ReciverId == userId).First());

                model.SaveChanges();
            }
        }

        public static void RemoveFriendOfferByReceiverLogin(int userId, string receiverLogin)
        {
            int receiverId = GetUserByLogin(receiverLogin).Id;

            using (var model = new TickCross())
            {
                model.FriendOffer.Remove(model.FriendOffer.Where(
                    x => x.SenderId == userId && x.ReciverId == receiverId).First());

                model.SaveChanges();
            }
        }

        private static List<FriendRequestModel> GetFriendReqModels(List<FriendOffer> reqs)
        {
            List<FriendRequestModel> res = new List<FriendRequestModel>();

            foreach (var req in reqs)
            {
                string senderLogin = GetUserLoginById((int)req.SenderId);
                string receiverLogin = GetUserLoginById((int)req.ReciverId);

                res.Add(new FriendRequestModel(senderLogin, receiverLogin));
            }
            return res;
        }

        private static int GetFriendRequestBySenderReceiverIds(int senderId, int receiverId)
        {
            using (var model = new TickCross())
            {
                return model.FriendOffer.Where(x => x.SenderId == senderId && x.ReciverId == receiverId).First().Id;
            }
        }

        private static int GetFriendRequestStatusId(FriendRequestStatus status)
        {
            using (var model = new TickCross())
            {
                return model.FriendReqStatus.Where(x => x.Name == status.ToString()).First().Id;
            }
        }

        public static void RemoveGameRequest(int senderId, int receiverId)
        {
            using (var model = new TickCross())
            {
                EntityModels.GameRequest req = model.GameRequest.Where(x => x.SenderId == senderId &&
                  x.ReciverId == receiverId).First();

                model.GameRequest.Remove(req);

                model.SaveChanges();
            }
        }

        public static List<Models.User> GetFriendsByUserId(int userId)
        {
            List<Models.User> res = new List<Models.User>();
            using (var model = new TickCross())
            {
                List<UserFriend> friends = model.UserFriend.Where(x => x.UserId == userId).ToList();

                for (int i = 0; i < friends.Count; i++)
                {
                    res.Add(GetUserModelByLogin(GetUserById((int)friends[i].FriendId).Login));
                }
            }
            return res;
        }

        public static List<Models.User> GetUsersToSendGameReq(int userId)
        {
            List<Models.User> res = new List<Models.User>();
            List<Models.User> friends = GetFriendsByUserId(userId);
            using (var model = new TickCross())
            {
                for (int i = 0; i < friends.Count; i++)
                {
                    int friendId = friends[i].Id;
                    bool hasRequest = model.GameRequest
                        .Any(x => x.SenderId == friendId || x.ReciverId == friendId);

                    if (!hasRequest)
                    {
                        res.Add(friends[i]);
                    }
                }
            }
            return res;
        }

        public static bool IsFriendRequestCanBeSent(int userId, string newFriendLogin)
        {
            Models.User newUser = GetUserByLogin(newFriendLogin);
            Models.User temUser = GetUserById(userId);

            //is user exist + is the same logins
            if (newUser is null || temUser.Login == newUser.Login) return false;

            using (var model = new TickCross())
            {
                bool check = model.FriendOffer.Where(x => (x.SenderId == userId && x.ReciverId == newUser.Id) ||
                (x.SenderId == userId && x.ReciverId == newUser.Id)).Any();

                return !check;
            }
        }

        private static bool IsUserStatusIsOnline(int userStatusId)
        {
            return GetUserStatusId("Online") == userStatusId;
        }
      
        private static int GetUserStatusId(string userStatus)
        {
            using(var model = new TickCross())
            {
                var status = model.UserStatus.FirstOrDefault(x => x.Name == userStatus);

                if (status == null)
                {
                    throw new InvalidOperationException($"User status '{userStatus}' not found in database.");
                }
                return status.Id;
            }
        }

        private static string GetUserStatById(int id)
        {
            using (var model = new TickCross())
            {
                return model.UserStatus.Where(x => x.Id == id).First().Name;
            }
        }

        private static UserStat GetUserStatus(string status)
        {
            for(int i = (int)UserStat.Offline; i <= (int)UserStat.Unavailable; i++)
            {
                if(status == ((UserStat)i).ToString())
                {
                    return ((UserStat)i);
                }
            }
            return UserStat.Unavailable;
        }

        public static bool IsPlayerCanPlayGame(int userId)
        {
            using(var model = new TickCross())
            {
                EntityModels.User user = model.User.Where(x => x.Id == userId).FirstOrDefault();
                if (user is null) return false;
                else if (user.StatusId == 3) return true;
            }
            return false;
        }
    }
}
