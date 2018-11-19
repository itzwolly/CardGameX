using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;

namespace CardGameLauncher.Scripts {
    public class FileHandler {
        public static string FolderLocation = @"D:/School/Year 3/Minor/Game/";

        public static bool CheckGameFilesAndDownloadIfNotExist(AuthenticationViewModel pVm) {
            Task<string> retrieve = Task.Run(async () => {
                return await WebServer.RetrieveFileHashesJson();
            });
            string phpResultJson = retrieve.Result;
            FileDataResult remoteResult = JsonConvert.DeserializeObject<FileDataResult>(phpResultJson);

            List<FileData> remoteData = new List<FileData>();
            for (int i = 0; i < remoteResult.FileNames.Length; i++) {
                remoteData.Add(new FileData(remoteResult.FileNames[i], remoteResult.Hashes[i], remoteResult.ParentFolder[i]));
            }

            List<FileData> localData = GetLocalFileData();

            if (remoteData.SequenceEqual(localData)) {
                return true;
            }

            IEnumerable<FileData> result = BagDifference<FileData>(remoteData, localData); //remoteData.Except(localData);

            int count = result.Count();
            if (count == 0) {
                return true;
            }
            
            Task<bool> download = Task.Run(async () => {
                return await WebServer.DownloadGameFiles(pVm, result);
            });

            return download.Result;
        }

        /// <summary>
        /// This checks the difference of two IEnumerable<T>'s based on their bags and not their sets.
        /// This means that if one list has 2 copies of an item and you subtract a set with one copy,
        /// there should be one copy left, rather than reducing all sequences into distinct sets before performing
        /// the subtraction, as Except() does.
        /// </summary>
        /// <typeparam name="FileData"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>A IEnumerable<T> which contains the different items of <typeparamref name="first"/> and <typeparamref name="second"/> including duplicates.</returns>
        public static IEnumerable<FileData> BagDifference<T>(IEnumerable<FileData> first, IEnumerable<FileData> second) {
            var dictionary = second.GroupBy(x => x.Hash).ToDictionary(group => group.Key, group => group.Count());

            foreach (var item in first) {
                int count;
                if (dictionary.TryGetValue(item.Hash, out count)) {
                    if (count - 1 == 0) {
                        dictionary.Remove(item.Hash);
                    } else {
                        dictionary[item.Hash] = count - 1;
                    }
                } else {
                    yield return item;
                }
            }
        }

        public static List<FileData> GetLocalFileData() {
            List<FileData> fileHashes = ProcessDirectory(FolderLocation);
            return fileHashes;
        }

        public static List<FileData> ProcessDirectory(string pTargetDirectory, List<FileData> pInitialSet = null) {
            if (pInitialSet == null) {
                pInitialSet = new List<FileData>();
            }

            Directory.CreateDirectory(pTargetDirectory);

            string[] fileEntries = Directory.GetFiles(pTargetDirectory);
            foreach (string fileName in fileEntries) {
                string hash = GetMD5HashToString(fileName);
                
                pInitialSet.Add(new FileData(fileName, hash, pTargetDirectory));
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
