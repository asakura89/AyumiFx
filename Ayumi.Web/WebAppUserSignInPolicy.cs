using System;

namespace WebApp {
    /// <summary>
    ///     Provide default implementation for IUserLoginPolicy.
    /// </summary>
    public class WebAppUserSignInPolicy : IUserSignInPolicy {
        /// <summary>
        ///     Compare user counter with default counter limit which is 5.
        /// </summary>
        /// <param name="userToCheck">WebAppUser</param>
        /// <returns>true/false</returns>
        public Boolean HasUserReachCounterLimit(WebAppUser userToCheck) => userToCheck.Counter >= 5;
    }
}