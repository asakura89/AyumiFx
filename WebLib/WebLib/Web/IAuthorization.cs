using System;

namespace WebLib.Web
{
    public interface IAuthorization
    {
        Boolean IsAuthorized();
        Boolean IsLoggedIn();
        void Login(String userId, String passwordString);
        void Logout();
    }
}