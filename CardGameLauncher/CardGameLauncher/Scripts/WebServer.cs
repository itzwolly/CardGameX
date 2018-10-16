using HtmlAgilityPack;
using Newtonsoft.Json;
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
        private const string GET_FILE_HASHES = "http://card-game.gearhostpreview.com/getfiledata.php/";
        private const string GAME_FOLDER = "http://card-game.gearhostpreview.com/game/";
        private const string SERVER_URL = "http://card-game.gearhostpreview.com/";

        public static async Task<string> RetrieveAuthenticationResultJson(string pUserName, string pPassword) {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("username", pUserName);
            postData.Add("password", pPassword);

            FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response = await client.PostAsync(AUTH_USER_URL, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public static async Task<string> RetrieveFileHashesJson() {
            HttpResponseMessage response = await client.GetAsync(GET_FILE_HASHES);
            string json = await response.Content.ReadAsStringAsync();
            
            return json;
        }

        public static async Task<bool> DownloadGameFiles(AuthenticationViewModel pVm, IEnumerable<FileData> pFilesToDownload) {
            using (WebClient client = new WebClient()) {
                client.DownloadProgressChanged += pVm.Client_DownloadProgressChanged;

                foreach (FileData file in pFilesToDownload) {
                    string extension = Path.GetExtension(file.FileName);
                    string location = FileHandler.FolderLocation;
                    Uri uri = new Uri(SERVER_URL + file.FileName);

                    if (file.ParentFolder != "") {
                        Directory.CreateDirectory(location + file.ParentFolder);
                    }

                    try {
                        byte[] data = await client.DownloadDataTaskAsync(uri);
                        File.WriteAllBytes(location + file.FileName, data);
                    } catch (WebException pWe) {
                        Console.WriteLine(pWe.Message + " : " + file.FileName);
                    } catch (Exception pE) {
                        Console.WriteLine(pE.Message + " : " + file.FileName);
                    }
                }

                return true;
            }
        }

        //public static async void DownloadGameFiles(AuthenticationViewModel pVm) {
        //    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(GAME_FOLDER);
        //    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse()) {
        //        using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
        //            string html = reader.ReadToEnd();
        //            HtmlDocument doc = new HtmlDocument();
        //            doc.LoadHtml(html);

        //            using (var client = new WebClient()) {
        //                client.DownloadProgressChanged += pVm.Client_DownloadProgressChanged;
        //                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a")) {
        //                    string href = link.Attributes["href"].Value;
        //                    if (href != "/") {
        //                        string path = SERVER_URL + href;
        //                        string location = "D:/School/Year 3/Minor" + href;
        //                        string extension = Path.GetExtension(path);
        //                        Uri uri = new Uri(path);

        //                        if (extension == "") {
        //                            Directory.CreateDirectory(location);
        //                        } else {
        //                            byte[] data = await client.DownloadDataTaskAsync(uri);
        //                            File.WriteAllBytes(location, data);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

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

    }
}

