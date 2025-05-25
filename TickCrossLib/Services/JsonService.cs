using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TickCrossLib.Services
{
    public static class JsonService
    {
        private static Dictionary<string, string> _dict = null;
        private static void SetStringParams(string fileName)
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.Parent.Parent.FullName;
            string clientDllPath = Path.Combine(parentPath, "TickCrossClient");
            string jsonFilePath = Path.Combine(clientDllPath, fileName);

            string json = File.ReadAllText(jsonFilePath);
            _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            //'B:\GitHub\TickCrossClientServer\TickCrossServer\params.json'.
        }

        public static int GetNumByName(string name)
        {
            const string _fileName = "params.json";
            SetStringParams(_fileName);
            //SetStringParams(JsonService.GetStringByName("ParamsFile")); //++-
            int.TryParse(_dict[name], out int res);

            return res;
        }

        public static string GetStringByName(string name)
        {
            const string _fileName = "params.json";
            SetStringParams(_fileName);
            //SetStringParams(JsonService.GetStringByName("ParamsFile")); //++-
            return _dict[name];
        }

        public static List<string> GetImageExpansions()
        {
            const string _fileName = "ImageExpansions.json";
            SetStringParams(_fileName);// JsonService.GetStringByName("ImagesFile")); //++-
            return _dict.Values.ToList();
        }

        
    }
}
