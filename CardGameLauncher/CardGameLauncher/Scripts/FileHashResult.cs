using System;
using System.Collections.Generic;

namespace CardGameLauncher.Scripts {
    public class FileHashResult {
        public string[] Hashes { get; set; }
        public string[] FileNames { get; set; }

        public FileHashResult(string [] pHashes) {
            if (Hashes == null) {
                Hashes = pHashes;
            }
        }
    }
}