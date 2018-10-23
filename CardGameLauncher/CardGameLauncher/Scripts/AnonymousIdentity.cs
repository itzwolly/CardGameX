using System;

namespace CardGameLauncher.Scripts {
    public class AnonymousIdentity : CustomIdentity {
        public AnonymousIdentity()
            : base(string.Empty, string.Empty, new string[] { }) { }
    }
}
