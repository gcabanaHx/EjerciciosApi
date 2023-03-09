using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Helpers
{
    public class Data
    {
        public static string EncodeToBase64(string text)
        {
            var bytes = Encoding.ASCII.GetBytes(text);

            return Convert.ToBase64String(bytes);
        }

        public static (string Key, string Value) GetAuthorizationHeader(string username, string password)
        {
            return ("Authorization", EncodeToBase64($"{username}:{password}"));
        }

        public static (string Key, string Value) GetAuthorizationHeaderValue(string usernameAndPassword)
        {
            return GetAuthorizationHeader(
                usernameAndPassword.Split(':').First(),
                usernameAndPassword.Split(':').Last());
        }

        public static Dictionary<string, string> GetAuthorizationHeader(string usernameAndPassword)
        {
            var data = GetAuthorizationHeaderValue(usernameAndPassword);
            return new Dictionary<string, string>()
            {
                { data.Key, data.Value }
            };
        }

        public static T getContent<T>(RestResponse response)
        {
            var _content = response.Content;
            return JsonConvert.DeserializeObject<T>(_content);
        }
    }
}