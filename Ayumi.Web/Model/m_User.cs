using System;

namespace WebApp.Model {
    public sealed class m_User {
        public Int32 Counter { get; set; }
        public Boolean ResetPassword { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
}