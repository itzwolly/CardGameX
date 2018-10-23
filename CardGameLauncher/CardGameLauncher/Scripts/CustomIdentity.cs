using System;
using System.Security.Principal;

namespace CardGameLauncher.Scripts {
    public class CustomIdentity : IIdentity {
        public CustomIdentity(string pName, string pEmail, string[] pRoles) {
            Name = pName;
            Email = pEmail;
            Roles = pRoles;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string[] Roles { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        #endregion
    }
}
