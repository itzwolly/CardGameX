using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public class FileHandler {
        public static string FolderLocation = "D:/School/Year 3/Minor/Game/";

        public static void DownloadGameFiles(AuthenticationViewModel pVm) {
            Task<string> task = Task.Run(async () => {
                return await WebServer.RetrieveFileHashesJson();
            });
            string phpResultJson = task.Result;
            FileHashResult remoteResult = JsonConvert.DeserializeObject<FileHashResult>(phpResultJson);

            HashSet<string> remoteHashes = new HashSet<string>(remoteResult.Hashes);
            HashSet<string> localHashes = GetLocalFileHashes();

            if (Compare(remoteHashes, localHashes)) {
                return;
            }

            localHashes.ExceptWith(remoteHashes); // remove all 

            WebServer.DownloadGameFiles(pVm, localHashes);
        }

        private static bool Compare(HashSet<string> pOne, HashSet<string> pOther) {
            return pOne.SetEquals(pOther);
        }

        public static HashSet<string> GetLocalFileHashes() {
            HashSet<string> fileHashes = ProcessDirectory(FolderLocation);
            return fileHashes;
        }

        public static HashSet<string> ProcessDirectory(string pTargetDirectory, HashSet<string> pInitialSet = null) {
            if (pInitialSet == null) {
                pInitialSet = new HashSet<string>();
            }

            string[] fileEntries = Directory.GetFiles(pTargetDirectory);
            foreach (string fileName in fileEntries) {
                string hash = GetMD5HashToString(fileName);
                pInitialSet.Add(hash);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(pTargetDirectory);
            foreach (string subdirectory in subdirectoryEntries) {
                ProcessDirectory(subdirectory, pInitialSet);
            }

            return pInitialSet;
        }

        public static string GetMD5HashToString(string pFileName) {
            if (pFileName != "") {
                byte[] hash = MD5Hash(pFileName);
                string result = "";
                foreach (byte b in hash) {
                    result += b.ToString("x2");
                }
                return result;
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
