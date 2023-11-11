using System;

namespace WK.AppUtility
{
    public abstract class WkManagerBase : MarshalByRefObject
    {
        protected DbAccess GetDbAccess(string functionName)
        {
            Console.WriteLine(functionName);
            return new DbAccess();
        }
    }
}
