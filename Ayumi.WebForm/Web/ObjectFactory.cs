using System;
using System.Collections.Generic;
using System.Data;

namespace WebLib.Web
{
    public class ObjectFactory : IObjectFactory
    {
        private static Dictionary<Type, Type> objectList = new Dictionary<Type, Type>();

        public A GetRegisteredObject<A>()
        {
            return GetRegisteredObject<A>(null);
        }

        public A GetRegisteredObject<A>(params Object[] constructorParamList)
        {
            Type abstracType = typeof (A);
            if (!objectList.ContainsKey(abstracType))
                return default(A);

            Type implementationType = objectList[abstracType];
            return (A) Activator.CreateInstance(implementationType, constructorParamList);
        }

        public void RegisterObject<A, I>()
        {
            Type abstracType = typeof (A);
            if (objectList.ContainsKey(abstracType))
                throw new DuplicateNameException("Object " + abstracType.Name + " was already defined.");

            I implementationObj = Activator.CreateInstance<I>();
            Type implementationType = implementationObj.GetType();
            objectList.Add(abstracType, implementationType);
        }
    }
}