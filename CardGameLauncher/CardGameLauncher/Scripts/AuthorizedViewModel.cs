using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public class AuthorizedViewModel : ObservableObject, IViewModel {
        public string Name {
            get {
                return "AuthorizedViewModel";
            }
        }
    }
}
