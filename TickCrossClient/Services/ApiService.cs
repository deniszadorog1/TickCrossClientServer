using Newtonsoft.Json;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TickCrossLib.Enums;
using TickCrossLib.Models.NonePlayable;
using TickCrossLib.Services;

namespace TickCrossClient.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _client;
        public static string _token;
        private static string? _host;
        static ApiService()
        {
            DotNetEnv.Env.Load();

            _host = Environment.GetEnvironmentVariable("localHost");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri(_host);
            //_client.BaseAddress = new Uri("https://localhost:7238/");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void SetToken(string token)
        {
            _token = token;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        //REGISTRATION
        public static async Task<bool> IsUserLoginExist(string login)
        {
            var requestData = new { SimpleReq = login };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Registration/CheckLogin", content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                jsonResponse = jsonResponse.Trim('"');
                return jsonResponse.ToString() == true.ToString();
            }

            return false;
        }

        public static async Task<bool> AddNewUser(string login, string password)
        {
            //string url = $"api/Registration/AddUser?login={login}&password={password}";

            var data = new { Login = login, Password = password };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Registration/AddUser", content);

            return response.IsSuccessStatusCode;
        }

        //LOGIN
        public static async Task<TickCrossLib.Models.User> GetLoggedUser(string login, string password)
        {
            //var qwe = DBService.GetLoggedUser(login, password);

            var data = new { UserLogin = login, UserPassword = password };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Login/GetLoggedUser", content);

            string jsonResponse = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonResponse)) return null;

            TickCrossLib.Models.User? user = jsonResponse is null ? null :
                JsonConvert.DeserializeObject<TickCrossLib.Models.User>(jsonResponse);

            return user;
        }

        public static async Task RemoveClosedGames(int userId)
        {
            var data = new { UserId = userId };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/Login/RemoveClosedGames"),
                RequestUri = new Uri($"{_host}api/Login/RemoveClosedGames"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);
        }

        //Get user enemies for game
        public static async Task<List<TickCrossLib.Models.User>?> GetEnemies(string login)
        {
            var response = await _client.GetAsync($"api/ChoseEnemy/GetPossibleEnemies?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<TickCrossLib.Models.User>>(jsonResponse);

            return users;
        }

        public static async Task<TickCrossLib.Models.User?> GetEnemyPlayer(string login)
        {
            var response = await _client.GetAsync($"api/ChoseEnemy/GetEnemy?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TickCrossLib.Models.User>(jsonResponse);
        }

        public static async Task<List<char>?> GetSigns()
        {
            var response = await _client.GetAsync($"api/ChoseEnemy/GetAllSigns");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<char>>(jsonResponse);
        }

        //User options
        public static async Task ChangeUserParams(TickCrossLib.Models.User user, string newLogin, string password)
        {
            DBService.ChangeUserParams(user.Login, newLogin, password);
        }

        //Friends 
        public static async Task<List<TickCrossLib.Models.User>?> GetUserFriends(string login)
        {
            var response = await _client.GetAsync($"api/Friends/GetFriends?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var friends = JsonConvert.DeserializeObject<List<TickCrossLib.Models.User>>(jsonResponse);

            return friends ?? new List<TickCrossLib.Models.User>();
        }

        public static async Task<List<TickCrossLib.Models.User>?> GetUsersThatNotFriend(string login)
        {
            var response = await _client.GetAsync($"api/Friends/GetUserToAddInFriends?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TickCrossLib.Models.User>>(jsonResponse);
        }

        public static async Task AddFriend(TickCrossLib.Models.User user, string toAddLogin)
        {
            var data = new
            {
                UserLogin = user.Login,
                ToAddLogin = toAddLogin
            };

            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"api/Friends/AddFriend", content);
        }

        public static async Task RemoveFriend(TickCrossLib.Models.User user, string login)
        {
            var data = new
            {
                UserLogin = user.Login,
                ToAddLogin = login
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/Friends/RemoveFriend"),
                RequestUri = new Uri($"{_host}api/Friends/RemoveFriend"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);

        }

        public static async Task<List<string>> GetFriendsLogins(int userId)
        {
            var response = await _client.GetAsync($"api/Friends/GetFriendsLogins?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(jsonResponse);
        }

        public static async Task<List<string>> GetUserLoginsToAddInFriends(int userId)
        {
            var response = await _client.GetAsync($"api/Friends/GetUserLoginsToAddInFriends?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(jsonResponse);
        }

        public static async Task AddRequest(int userId, string receiverLogin)
        {
            var data = new
            {
                UserId = userId,
                ReceiverLogin = receiverLogin
            };

            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync($"api/Friends/AddFriendOffer", content);
        }

        public static async Task<List<FriendRequestModel>> GetFriendReqsSentByUser(int userId)
        {
            var response = await _client.GetAsync($"api/Friends/GetReqsSentByUser?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FriendRequestModel>>(jsonResponse);
        }

        public static async Task<List<FriendRequestModel>> GetFriendReqsSentToUser(int userId)
        {
            var response = await _client.GetAsync($"api/Friends/GetReqsSentToUser?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FriendRequestModel>>(jsonResponse);
        }

        public static async Task DeleteFriendReqBySenderLogin(int userId, string senderLogin)
        {
            var data = new
            {
                ReceiverId = userId,
                SenderLogin = senderLogin
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/Friends/RemoveReqBySenderLogin"),
                RequestUri = new Uri($"{_host}api/Friends/RemoveReqBySenderLogin"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            await _client.SendAsync(request);
        }

        public static async Task DeleteFriendReqByReceiverLogin(int userId, string receiverLogin)
        {
            var data = new
            {
                SenderId = userId,
                ReceiverLogin = receiverLogin
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/Friends/RemoveReqByReceiverLogin"),
                RequestUri = new Uri($"{_host}api/Friends/RemoveReqByReceiverLogin"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            await _client.SendAsync(request);
        }

        public static async Task<bool> IsFriendRequestCanBeSent(int userId, string newFriendLogin)
        {
            var response = await _client.GetAsync($"api/Friends/IsFriendRequestCanBeSent?userId={userId}&newFriendLogin={newFriendLogin}");
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public static async Task<bool> IsUserAlreadyAFriend(int userId, string newFriendLogin)
        {
            var response = await _client.GetAsync($"api/Friends/IsUserIsAlreadyAFriend?userId={userId}&newFriendLogin={newFriendLogin}");
            if (!response.IsSuccessStatusCode) return true;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        //Game Request
        public static async Task AddNewGameRequest(string senderLogin, string receiverLogin,
            char enemySign, char userSign, TickCrossLib.Enums.RequestStatus status)
        {
            var data = new
            {
                SenderLogin = senderLogin,
                ReceiverLogin = receiverLogin,
                EnemySign = enemySign,
                UserSign = userSign,
                Status = status
            };

            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"api/GameReq/AddGameRequest", content);
        }

        public static async Task<TickCrossLib.Models.GameRequest?> GetFirstGameRequest(string login)
        {
            var response = await _client.GetAsync($"api/GameReq/GetGameRequest?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TickCrossLib.Models.GameRequest>(jsonResponse);
        }

        public static async Task<TickCrossLib.Models.GameRequest?> GetAcceptedGameRequest(string login)
        {
            var response = await _client.GetAsync($"api/GameReq/GetAcceptedGameRequest?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TickCrossLib.Models.GameRequest>(jsonResponse);
        }


        public static async void AddTempGameInDB(int gameId)
        {
            var data = new { GameId = gameId };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/AddNewTempGame", content);
        }

        public static async Task<bool> IsUserIsOnline(string login)
        {
            var response = await _client.GetAsync($"api/GameReq/IsUserIsOnline?login={login}");
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var isLogged = JsonConvert.DeserializeObject<bool>(jsonResponse);

            return isLogged;
        }

        public static async Task<bool> IsUserInGame(string login)
        {
            var response = await _client.GetAsync($"api/GameReq/IsUserIsInGame?login={login}");
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var isLogged = JsonConvert.DeserializeObject<bool>(jsonResponse);

            return isLogged;
        }

        public static async void SetUserLoginStatus(int userId, UserStat isLogged)
        {
            var data = new
            {
                UserId = userId,
                Stat = isLogged
            };

            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync($"api/Login/SetUserLoginStatus", content);
        }

        public static async Task<bool> IsUserIsLoggedById(int id)
        {
            var response = await _client.GetAsync($"api/Login/IsUserLogged?userId={id}");
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var isLogged = JsonConvert.DeserializeObject<bool>(jsonResponse);

            return isLogged;
        }

        public static async Task<bool> IsUserLoggedOnLoginPage(int userId)
        {
            var response = await _client.GetAsync($"api/Login/IsUserLoggedAtLoginPage?userId={userId}");
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var isLogged = JsonConvert.DeserializeObject<bool>(jsonResponse);

            return isLogged;
        }

        public static async Task<(int?, int?)> GetMoveCord(int gameId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetCordToMove?gameId={gameId}");

            if (!response.IsSuccessStatusCode) return (-1, -1);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var cord = JsonConvert.DeserializeObject<TickCrossLib.Models.HelpModels.Cord>(jsonResponse);

            return (cord.X, cord.Y);
        }

        public static async Task SetStatusForTempGame(GameEnded status, int gameId)
        {
            var data = new { Status = status.ToString(), GameId = gameId };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/SetTempGameStatus", content);

            //return response.IsSuccessStatusCode;
        }

        public static async Task SetMovePointInTempGame((int, int) cord, int gameId)
        {
            var data = new { Cord = new TickCrossLib.Models.HelpModels.Cord { X = cord.Item1, Y = cord.Item2 }, GameId = gameId };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/ChangeTempGameMoveCord", content);
        }
/*        public class Point //++-
        {
            public int X { get; set; }
            public int Y { get; set; }
        }*/


        public static async Task<string?> GetTempGameStatus(int gameId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetTempGameStatus?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<string>(jsonResponse);
        }

        public static async Task<TickCrossLib.Models.User?> GetUserByLogin(string login)
        {
            var response = await _client.GetAsync($"api/GameReq/GetUserByLogin?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TickCrossLib.Models.User>(jsonResponse);
        }

        public static async Task<TickCrossLib.Models.User?> GetUserById(int id)
        {
            var response = await _client.GetAsync($"api/GameReq/GetUserById?id={id}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TickCrossLib.Models.User>(jsonResponse);
        }

        public static async Task AddGameInDB(TickCrossLib.Models.Game game)
        {
            var data = new { Game = game };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/AddGame", content);
        }

        public static async Task SetRequestStatus(TickCrossLib.Models.GameRequest req,
            TickCrossLib.Enums.RequestStatus newStatus)
        {
            var data = new { Request = req, Status = newStatus };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/SetRequestStatus", content);
        }

        public static async Task<string?> GetReqStatus(int reqId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetRequestStatus?reqId={reqId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(jsonResponse);
        }

        public static async Task<int?> GetLastGameId()
        {
            var response = await _client.GetAsync($"api/GameReq/GetLastGameId");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public static async Task SetStepperForTempGame(int gameId, int newStepperId)
        {
            var data = new { GameId = gameId, NewStepperId = newStepperId };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/SetStepperInTempGame", content);
        }

        public static async Task<int?> GetTempGameStepperId(int gameId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetTempGameStepperId?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int?>(jsonResponse);

        }

        public static async Task<TickCrossLib.Models.GameRequest> GetGameRequest(string senderLogin, string receiverLogin)
        {
            TickCrossLib.Models.User receiver = DBService.GetUserByLogin(receiverLogin);
            TickCrossLib.Models.User sender = DBService.GetUserByLogin(senderLogin);

            var response = await _client.GetAsync(
                $"api/MainMenu/GetGameRequest?receiverId={receiver.Id}&senderId={sender.Id}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TickCrossLib.Models.GameRequest>(jsonContent);
        }

        public static async Task SetGameResult(int gameId, int? winnerId, bool? isDraw)
        {
            var data = new
            {
                GameId = gameId,
                WinnerId = winnerId,
                IsDraw = isDraw
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/SetGameResult", content);
        }

        public static async Task RemoveUserRequests(int userId)
        {
            var data = new
            {
                UserId = userId
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/GameReq/RemoveRequests"),
                RequestUri = new Uri($"{_host}api/GameReq/RemoveRequests"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);
        }

        public static async Task RemoveTempGame(int gameId)
        {
            var data = new
            {
                GameId = gameId
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/GameReq/RemoveTempGame"),
                RequestUri = new Uri($"{_host}api/GameReq/RemoveTempGame"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);
        }

        public static async Task SetGameCanceledStatus(int gameId)
        {
            var data = new
            {
                GameId = gameId,
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Game/SetGameCanceledStatus", content);
        }

        public static async Task<bool> IsGameBeenCanceled(int gameId)
        {
            var response = await _client.GetAsync($"api/Game/IsGameBeenCanceled?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(jsonResponse);
        }

        public static async Task<bool> IsTempGameIsExist(int gameId)
        {
            var response = await _client.GetAsync($"api/Game/IsTempGameIsExist?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return false;
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<bool>(jsonResponse);
        }

        //MAIN MENU
        public static async Task<int> GetUserWinsAmount(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetUserWinsAmount?userId={userId}");
            if (!response.IsSuccessStatusCode) return -1;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public static async Task<int> GetUserLosesAmount(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetUserLosesAmount?userId={userId}");
            if (!response.IsSuccessStatusCode) return -1;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public static async Task<int> GetUserDrawsAmount(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetUserDrawsAmount?userId={userId}");
            if (!response.IsSuccessStatusCode) return -1;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public static async Task<int> GetUserGamesAmount(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetUserGamesAmount?userId={userId}");
            if (!response.IsSuccessStatusCode) return -1;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public static void SetEnv()
        {
            DotNetEnv.Env.Load();

            var message = Environment.GetEnvironmentVariable("ASD");
            //return message;
        }

        public static async Task<List<GameRequestModel>> GetGameRequestsSentToUser(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetUserReqsSentToUser?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GameRequestModel>>(jsonResponse);
        }

        public static async Task<List<GameRequestModel>> GetRequestsSentByUser(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetGameReqsSentByUser?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GameRequestModel>>(jsonResponse);
        }

        public static async Task RemoveGameRequest(int userId, string enemyLogin)
        {
            var data = new
            {
                UserId = userId,
                EnemyLogin = enemyLogin
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/GameReq/RemoveGameRequest"),
                RequestUri = new Uri($"{_host}api/GameReq/RemoveGameRequest"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);
        }

        public static async Task RejectGameRequest(int userId, string senderLogin)
        {
            var data = new
            {
                UserId = userId,
                EnemyLogin = senderLogin
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                //RequestUri = new Uri("https://localhost:7238/api/GameReq/RejectGameRequest"),
                RequestUri = new Uri($"{_host}api/GameReq/RejectGameRequest"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);
        }

        public static async Task<List<TickCrossLib.Models.User>> GetUsersToSendGameReq(int userId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetUsersToSendGameRequest?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TickCrossLib.Models.User>>(jsonResponse);
        }

        public static async Task<GameEnded> GetGameStatus(int gameId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetGameStatus?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return GameEnded.InProgress;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GameEnded>(jsonResponse);
        }

        public static async Task<string> GetGameWinnerLogin(int gameId)
        {
            var response = await _client.GetAsync($"api/GameReq/GetWinnerLogin?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return JsonService.GetStringByName("DefaultNullValString"); //++-

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(jsonResponse);
        }

        public static async Task<List<GameResult>> GetGameHistory(int userId)
        {
            var response = await _client.GetAsync($"api/MainMenu/GetGameHistory?userId={userId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonRequest = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GameResult>>(jsonRequest);
        }

        //TimerController 

        public static async void StartMoveTimer(int gameId)
        {
            var data = new
            {
                GameId = gameId,
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Timer/StartTimer", content);
        }

        public static async Task<double?> GetLeftTime(int gameId)
        {
            var response = await _client.GetAsync($"api/Timer/GetTimeLeft?gameId={gameId}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<double>(jsonResponse);
        }
    }
}
