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
using TickCrossLib.EntityModel;
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
        public static async Task<TickCrossLib.Models.User?> GetLoggedUser(string login, string password)
        {
            var data = new { UserLogin = login, UserPassword = password };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Login/GetLoggedUser", content).ConfigureAwait(false);

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
        public static async Task<List<TickCrossLib.Models.User>> GetUserFriends(string login)
        {
            var response = await _client.GetAsync($"api/Friends/GetFriends?login={login}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var friends = JsonConvert.DeserializeObject<List<TickCrossLib.Models.User>>(jsonResponse);

            return friends ?? new List<TickCrossLib.Models.User>();
        }

        public static async Task<List<TickCrossLib.Models.User>> GetUsersThatNotFriend(string login)
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

            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //To ask about remove
            var response =  await _client.PostAsync($"api/Friends/RemoveFriend", content);
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
