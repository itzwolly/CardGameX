using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public class FileHelper {
        public static string GetFileLocation() {
            return string.Empty;
        }

        private static bool FileEquals(string pFilePath, string pFilePathToCompare) {
            if (Path.GetFileName(pFilePath) != Path.GetFileName(pFilePathToCompare) &&
                Path.GetExtension(pFilePath) != Path.GetExtension(pFilePathToCompare)) {
                return false;
            }

            byte[] file = File.ReadAllBytes(pFilePath);
            byte[] fileToCompare = File.ReadAllBytes(pFilePathToCompare);

            if (file.Length == fileToCompare.Length) {
                for (int i = 0; i < file.Length; i++) {
                    if (file[i] != fileToCompare[i]) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
