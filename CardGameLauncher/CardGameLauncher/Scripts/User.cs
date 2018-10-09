using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public class User {
        public User(string pUsername, string pEmail, string[] pRoles) {
            Username = pUsername;
            Email = pEmail;
            Roles = pRoles;
        }

        public string Username {
            get;
            set;
        }

        public string Email {
            get;
            set;
        }

        public string[] Roles {
            get;
            set;
        }
    }
}
