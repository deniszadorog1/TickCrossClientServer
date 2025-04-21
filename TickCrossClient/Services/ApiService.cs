using Microsoft.Identity.Client;
using Microsoft.Xaml.Behaviors.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Pages;
using TickCrossLib.EntityModels;
using TickCrossLib.Enums;
using TickCrossLib.Models;
using TickCrossLib.Services;



namespace TickCrossClient.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _client;


        static ApiService()
        {
            DotNetEnv.Env.Load();

            string? host = Environment.GetEnvironmentVariable("localHost");


            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri("https://localhost:7238/");

            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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
                return jsonResponse == "1";
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

            return;
            var date = new
            {
                OldLogin = user.Login,
                OldPassword = user.Password,
                NewLogin = newLogin,
                NewPassword = password
            };

            var json = JsonConvert.SerializeObject(date);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/Options/ChangeUserParams", content);

            if (!response.IsSuccessStatusCode) throw new DataException("Cant be updated!!!");
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

            /*            string json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        //To ask about remove
                        var response =  await _client.DeleteAsync($"api/Friends/RemoveFriend?userLogin={content}&toRemove={login}");
            */

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://localhost:7238/api/Friends/RemoveFriend"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);

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

        public static async void AddTempGameInDB(int gameId)
        {
            var data = new { GameId = gameId };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/AddNewTempGame", content);
        }

        public static async Task<bool> IsUserIsLogged(string login)
        {
            var response = await _client.GetAsync($"api/GameReq/IsUserIsLoggedIn?login={login}");
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var isLogged = JsonConvert.DeserializeObject<bool>(jsonResponse);

            return isLogged;
        }

        public static async Task<bool> IsUserIsLoggedById(int id)
        {
            var response = await _client.GetAsync($"api/GameReq/IsUserIsLoggedInById?login={id}");
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
            var data = new { Cord = new Point { X = cord.Item1, Y = cord.Item2 }, GameId = gameId };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/ChangeTempGameMoveCord", content);
        }
        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

        }


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
            var data = new {Game = game};

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/GameReq/AddGame", content);
        }

        public static async Task SetGameResult(TickCrossLib.Enums.GameEnded gameStat)
        {

        }

        public static async Task SetRequestStatus(TickCrossLib.Models.GameRequest req,
            TickCrossLib.Enums.RequestStatus newStatus)
        {
            var data = new { Request = req, Status = newStatus};

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

        //MAIN MENU
        public static async Task<List<TickCrossLib.Models.User>> GetAllUsers()
        {
            List<TickCrossLib.Models.User> result = new List<TickCrossLib.Models.User>();

            return result;
        }

        public static void SetEnv()
        {
            DotNetEnv.Env.Load();

            var message = Environment.GetEnvironmentVariable("ASD");
            //return message;
        }
    }
}
