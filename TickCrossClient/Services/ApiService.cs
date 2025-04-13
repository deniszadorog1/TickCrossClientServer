using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickCrossClient.Services
{
    public static class ApiService
    {
        public static string SetEnv()
        {
            DotNetEnv.Env.Load();

            var message = Environment.GetEnvironmentVariable("ASD");
            return message;
        }

    }
}
