using System;
using System.Collections.Generic;

namespace CardGameLauncher.Scripts {
    public class FileDataResult {
        public string[] Hashes { get; set; }
        public string[] FileNames { get; set; }
        public string[] ParentFolder { get; set; }

        public FileDataResult(string[] pFileNames, string[] pHashes) {
            FileNames = pFileNames;
            Hashes = pHashes;
        }

        public Dictionary<string, string> GetFileData() {
            Dictionary<string, string> fileData = new Dictionary<string, string>();
            for (int i = 0; i < Hashes.Length; i++) {
                fileData.Add(FileNames[i], Hashes[i]);
            }

            return fileData;
        }
    }
}