﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public static class WebServer {
        private static readonly HttpClient client = new HttpClient();
        private const string AUTH_USER_URL = "http://card-game.gearhostpreview.com/authenticate.php";
        private const string DOWNLOAD_GAME_FILES = "http://card-game.gearhostpreview.com/checkfiles.php/";
        private const string GAME_FOLDER = "http://card-game.gearhostpreview.com/game/";
        private const string SERVER_URL = "http://card-game.gearhostpreview.com";

        public static async Task<string> RetrieveAuthenticationResultJson(string pUserName, string pPassword) {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("username", pUserName);
            postData.Add("password", pPassword);

            FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response = await client.PostAsync(AUTH_USER_URL, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public static async void DownloadGameFiles(AuthenticationViewModel pVm) {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(GAME_FOLDER);
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse()) {
                using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
                    string html = reader.ReadToEnd();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    using (var client = new WebClient()) {
                        client.DownloadProgressChanged += pVm.Client_DownloadProgressChanged;
                        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a")) {
                            string href = link.Attributes["href"].Value;
                            if (href != "/") {
                                string path = SERVER_URL + href;
                                string location = "D:/School/Year 3/Minor" + href;
                                string extension = Path.GetExtension(path);
                                Uri uri = new Uri(path);

                                if (extension == "") {
                                    Directory.CreateDirectory(location);
                                } else {
                                    byte[] data = await client.DownloadDataTaskAsync(uri);
                                    File.WriteAllBytes(location, data);
                                }
                            }
                        }
                    }
                }
            }
        }

        //public static async void DownloadGameFiles(string pLocation) {
        //    Dictionary<string, string> postData = new Dictionary<string, string>();
        //    postData.Add("location", pLocation);

        //    FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
        //    HttpResponseMessage response = await client.GetAsync(DOWNLOAD_GAME_FILES);
        //    Stream file = await response.Content.ReadAsStreamAsync();

        //    //"\nNotice: Undefined index: location in C:\\home\\site\\wwwroot\\checkfiles.php on line 2\n\nFatal error: Call to a member function getSize() on a non-object in C:\\home\\site\\wwwroot\\checkfiles.php on line 28\n"
        //    //"\nNotice: Undefined index: location in C:\\home\\site\\wwwroot\\checkfiles.php on line 2\n\nWarning: readfile(GameCard_Game_Photon_Prototype.exe): failed to open stream: No such file or directory in C:\\home\\site\\wwwroot\\checkfiles.php on line 29\n"
        //    using (StreamReader reader = new StreamReader(file)) {
        //        string strContent = reader.ReadToEnd();
        //    }
        //}

        //public static async void ValidateGameFiles(string pHash, string pFullFileName, string pParentFolder) {
        //    Dictionary<string, string> postData = new Dictionary<string, string>();
        //    postData.Add("hash", pHash);
        //    postData.Add("fullfilename", pFullFileName);
        //    postData.Add("parent", pParentFolder);

        //    FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
        //    HttpResponseMessage response = await client.GetAsync(DOWNLOAD_GAME_FILES);
        //    Stream file = await response.Content.ReadAsStreamAsync();

        //    Console.WriteLine(file);
        //}

        public static string GetMD5HashToString(string pFileName) {
            if (pFileName != "") {
                byte[] hash = WebServer.MD5Hash(pFileName);
                string s = "";
                foreach (byte bit in hash) {
                    s += bit.ToString("x2");
                }
                return s;
            }
            return "";
        }

        private static byte[] SHA256Hash(string pFilePath) {
            using (SHA256 SHA256 = SHA256Managed.Create()) {
                using (FileStream fileStream = File.OpenRead(pFilePath))
                    return SHA256.ComputeHash(fileStream);
            }
        }

        private static byte[] MD5Hash(string pFilePath) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(pFilePath)) {
                    return md5.ComputeHash(stream);
                }
            }
        }
    }
}

