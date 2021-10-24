using System;

namespace WebApp {
    /// <summary>
    ///     Used for validate User's readiness when authentication.
    /// </summary>
    public interface IUserSignInPolicy {
        /// <summary>
        ///     Counter limit is user defined. So use this interface to check user counter.
        /// </summary>
        /// <param name="userToCheck">WebAppUser</param>
        /// <returns>true/false</returns>
        Boolean HasUserReachCounterLimit(WebAppUser userToCheck);
    }
}