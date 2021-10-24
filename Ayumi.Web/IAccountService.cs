using System;
using WebApp.ViewModel;

namespace WebApp {
    /// <summary>
    ///     All about login. authentication and authorization / permission.
    /// </summary>
    public interface IAccountService {
        /// <summary>
        ///     Check whether user is authenticated / logged in.
        /// </summary>
        /// <returns>true/false</returns>
        Boolean IsAuthenticated();

        /// <summary>
        ///     Check whether user is authorized for certain module.
        /// </summary>
        /// <param name="activityId">String</param>
        /// <returns>true/false</returns>
        Boolean IsAuthorized(String activityId);

        /// <summary>
        ///     Log In into system.
        /// </summary>
        /// <param name="username">String</param>
        /// <param name="password">String</param>
        void SignIn(String username, String password);

        /// <summary>
        ///     Log Out current user.
        /// </summary>
        void SignOut();

        /// <summary>
        ///     Change user password only excluding other info.
        /// </summary>
        /// <param name="tenant">String</param>
        /// <param name="changePasswordInfo">WebAppChangePassword</param>
        void ChangePassword(String tenant, WebAppChangePassword changePasswordInfo);
    }
}