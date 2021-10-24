using System;

namespace WebApp.ViewModel {
    public sealed class WebAppChangePassword {
        public String Username { get; set; }
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
    }
}