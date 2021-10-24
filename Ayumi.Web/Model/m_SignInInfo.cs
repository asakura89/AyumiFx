using System;

namespace WebApp.Model {
    public sealed class m_SignInInfo {
        public DateTime SignInTime { get; set; }

        public DateTime? SignOutTime { get; set; }

        public String Username { get; set; }

        public String SessionId { get; set; }

        public String IpAddress { get; set; }
    }
}