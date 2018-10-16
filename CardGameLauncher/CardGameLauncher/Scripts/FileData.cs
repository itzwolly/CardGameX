using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public class FileData : IEqualityComparer<FileData> {
        public string FileName { get; private set; }
        public string Hash { get; private set; }
        public string ParentFolder { get; private set; }

        public FileData(string pFileName, string pHash, string pParentFolder) {
            FileName = pFileName;
            Hash = pHash;
            ParentFolder = pParentFolder;
        }

        public bool Equals(FileData x, FileData y) {
            return (x != null && y != null) && x.Hash.Equals(y.Hash);
        }

        public int GetHashCode(FileData obj) {
            return obj.FileName.GetHashCode() ^ obj.Hash.GetHashCode() ^ obj.ParentFolder.GetHashCode();
        }
    }
}
