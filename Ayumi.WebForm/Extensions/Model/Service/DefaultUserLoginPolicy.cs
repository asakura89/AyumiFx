using System;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    /// <summary>
    /// Provide default implementation for IUserLoginPolicy.
    /// </summary>
    public class DefaultUserLoginPolicy : IUserLoginPolicy
    {
        /// <summary>
        /// Compare user counter with default counter limit which is 5.
        /// </summary>
        /// <param name="userToCheck">IUser</param>
        /// <returns>true/false</returns>
        public Boolean IsUserTouchCounterLimit(IUser userToCheck)
        {
            return userToCheck.Counter >= 5;
        }
    }
}