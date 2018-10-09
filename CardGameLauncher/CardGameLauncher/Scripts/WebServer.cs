using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public static class WebServer {
        private static readonly HttpClient client = new HttpClient();
        private const string AUTH_USER_URL = "http://card-game.gearhostpreview.com/authenticate.php";

        public static async Task<string> RetrieveAuthenticationResultJson(string pUserName, string pPassword) {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("username", pUserName);
            postData.Add("password", pPassword);

            FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync(AUTH_USER_URL, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}

