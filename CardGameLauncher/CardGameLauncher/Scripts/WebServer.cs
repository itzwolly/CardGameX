using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public static class WebServer {
        private static readonly HttpClient client = new HttpClient();
        private const string AUTH_USER_URL = "http://card-game.gearhostpreview.com/authenticate.php";
        private const string DOWNLOAD_FILES_URL = "";

        public static async Task<string> RetrieveAuthenticationResultJson(string pUserName, string pPassword) {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("username", pUserName);
            postData.Add("password", pPassword);

            FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response = await client.PostAsync(AUTH_USER_URL, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public static async void CheckGameFiles() {
            HttpResponseMessage response = await client.GetAsync(DOWNLOAD_FILES_URL);
            EntityTagHeaderValue eTag = response.Headers.ETag;
            string content = await response.Content.ReadAsStringAsync();


        }
    }
}

