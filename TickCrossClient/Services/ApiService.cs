using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TickCrossLib.EntityModel;

namespace TickCrossClient.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _client;


        static ApiService()
        {
            DotNetEnv.Env.Load();

            string host = Environment.GetEnvironmentVariable("localHost");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true // Игнорирует ошибки SSL, но с дополнительной проверкой
            };


            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7238"),
                
            };
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

            var response = await _client.PostAsync("api/Login/GetLoggedUser", content);

            MessageBox.Show("Got response");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonResponse)) return null;

            TickCrossLib.Models.User? user = jsonResponse is null ? null :
                JsonConvert.DeserializeObject<TickCrossLib.Models.User>(jsonResponse);

            return null;
        }


        //MAIN MENU

        public static async Task<List<TickCrossLib.Models.User>> GetAllUsers()
        {
            List<TickCrossLib.Models.User> result = new List<TickCrossLib.Models.User>();

            return result;
        }



        public static async void AddFriend()
        {

        }

        //GAME




        public static async void SetGameInDB()
        {

        }


        public static void SetEnv()
        {
            DotNetEnv.Env.Load();

            var message = Environment.GetEnvironmentVariable("ASD");
            //return message;
        }
    }
}
