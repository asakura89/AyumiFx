using System;
using System.Reflection;
using WK.RemotingInterface;

namespace WK.RemotingManager
{
    public class ObjectFactory : MarshalByRefObject, IObjectFactory
    {
        public object CreateObject(string assemblyName, string fqn)
        {
            var asmPath = GetAssemblyPath(assemblyName);
            Assembly assembly = Assembly.LoadFile(asmPath);

            object obj = assembly.CreateInstance(fqn);
            return obj;
        }

        private string GetAssemblyPath(string assemblyName)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName + ".dll");
        }
    }
}
