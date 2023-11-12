using System;

namespace Reflx {
    public class TypeAndAssembly {
        public String Type { get; }
        public String Assembly { get; }

        public TypeAndAssembly(String type, String assembly) {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            if (String.IsNullOrEmpty(assembly))
                throw new ArgumentNullException(nameof(assembly));

            Type = type;
            Assembly = assembly;
        }
    }
}
