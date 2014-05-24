using System;

namespace WebLib.Web
{
    public interface IAuthorization
    {
        Boolean IsAuthorized();
        void Login(String userId, String passwordString);
        void Logout();
    }
}