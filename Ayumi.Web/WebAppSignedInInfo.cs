using System;
using System.Collections.Generic;

namespace WebApp {
    public class WebAppSignedInInfo {
        public String Username { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public IList<String> GroupList { get; set; }

        public String SessionId { get; set; }
        public String IpAddress { get; set; }
        public DateTime SignInTime { get; set; }
        public DateTime SignOutTime { get; set; }
    }
}