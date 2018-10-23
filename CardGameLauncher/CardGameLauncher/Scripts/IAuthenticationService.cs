using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public interface IAuthenticationService {
        User AuthenticateUser(string pUsername, string pPassword);
    }
}
