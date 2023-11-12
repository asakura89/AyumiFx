using System;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    /// <summary>
    /// Used for validate User's readiness when authentication.
    /// </summary>
    public interface IUserLoginPolicy
    {
        /// <summary>
        /// Counter limit is user defined. So use this interface to check user counter.
        /// </summary>
        /// <param name="userToCheck">IUser</param>
        /// <returns>true/false</returns>
        Boolean IsUserTouchCounterLimit(IUser userToCheck);
    }
}