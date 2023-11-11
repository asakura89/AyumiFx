using System;

namespace WebLib.Constant
{
    internal sealed class CommonException
    {
        internal const String NotAuthorized = "You are not authorized to access this page.";
        internal const String ContactAdmin = "Your account has been locked. Please contact your administrator.";
        internal const String CantLogin = "Unknown user or bad password.";
    }
}