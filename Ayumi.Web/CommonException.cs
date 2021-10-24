using System;

namespace WebApp {
    public sealed class CommonException {
        public const String NotAuthorized = "You are not authorized to access this page.";
        public const String ContactAdmin = "Your account has been locked. Please contact your administrator.";
        public const String InvalidUser = "Invalid user.";
        public const String CantLogin = "Unknown user or bad password.";
        public const String BadPassword = "Bad password.";
    }
}