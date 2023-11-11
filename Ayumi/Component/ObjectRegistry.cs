using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ayumi.Component {
    public class ObjectInstanceNotFoundException : Exception {
        public ObjectInstanceNotFoundException(String message) : base(message) { }

        public ObjectInstanceNotFoundException(String message, Exception innerException) : base(message, innerException) { }
    }

    public static class ObjectRegistry {
        static readonly IDictionary<Type, Func<Object>> objects = new Dictionary<Type, Func<Object>>();

        public static A Get<A>() => (A) GetInstance(typeof(A));

        public static void Register<A, I>() where I : A => objects[typeof(A)] = () => GetInstance(typeof(I));

        public static void Register<I>(Func<I> instanceCreator) => objects[typeof(I)] = () => instanceCreator();

        public static void Register<I>(I instance) => objects[typeof(I)] = () => instance;

        public static T TryResolve<T>() => (T) GetInstance(typeof(T));

        public static T Resolve<T>() {
            T resolved = TryResolve<T>();
            if (resolved == null)
                throw new ObjectInstanceNotFoundException($"'{typeof(T).FullName}' instance can't be found or can't be created.");

            return resolved;
        }

        static Object GetInstance(Type abstractType) {
            if (objects.TryGetValue(abstractType, out Func<Object> creator))
                return creator();

            if (!abstractType.IsAbstract)
                return CreateInstance(abstractType);

            return null;
        }

        static Object CreateInstance(Type implType) {
            ConstructorInfo ctor = implType.GetConstructors()[0];
            ParameterInfo[] ctorParams = ctor.GetParameters();
            if (ctorParams.Length == 0)
                return Activator.CreateInstance(implType);

            Object[] parameters = ctorParams
                .Select(param => param.ParameterType)
                .Select(GetInstance)
                .ToArray();

            return Activator.CreateInstance(implType, parameters);
        }
    }
}