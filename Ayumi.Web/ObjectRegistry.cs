namespace WebApp {
    public static class ObjectRegistry {
        /*private static readonly Dictionary<String, KeyValuePair<Type, Type>> objects = new Dictionary<String, KeyValuePair<Type, Type>>();

        private static String BuildAutoName(Type aType)
        {
            return new StringBuilder()
                .Append("Named.")
                .Append(aType.Name)
                .ToString();
        }

        public static void Register<Abstract, Implementation>() where Implementation : Abstract
        {
            String autoName = BuildAutoName(typeof(Abstract));
            Register<Abstract, Implementation>(autoName);
        }

        public static void Register<Abstract, Implementation>(String name) where Implementation : Abstract
        {
            Type abstractType = typeof(Abstract);
            Type implType = typeof(Implementation);

            if (!objects.ContainsKey(name))
                objects.Add(name, new KeyValuePair<Type, Type>(abstractType, implType));
        }

        public static Abstract Resolve<Abstract>()
        {
            return GetRegisteredObject<Abstract>(null);
        }

        public static Abstract Resolve<Abstract>(params Object[] constructorParams)
        {

            Type implType = objects[typeof(Abstract)];
            return (Abstract)Activator.CreateInstance(implType, constructorParams);
        }

        public static Abstract ResolveNamed<Abstract>(String name)
        {
            String autoName = BuildAutoName(typeof(Abstract));
            return ResolveNamed<Abstract>(autoName);
        }

        public static Abstract ResolveNamed<Abstract>(String name, params Object[] constructorParams)
        {
            if (objects.ContainsKey(name))
            {
                Type implType = objects[name];
                return (Abstract)Activator.CreateInstance(implType, constructorParams);
            }
        }*/
    }
}