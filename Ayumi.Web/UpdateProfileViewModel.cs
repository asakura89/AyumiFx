using System;

namespace WebApp {
    public class UpdateProfileViewModel {
        public String Email { get; set; }
        public Boolean IsChangePassword { get; set; }
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
        public String ConfirmPassword { get; set; }
    }
}