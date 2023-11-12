using System.Linq;
using System.Reflection;

namespace Reflx {
    public class PropertyHelper : IPropertyHelper {
        public TAttribute GetDecorator<TAttribute>(PropertyInfo property) =>
            property == null ? default(TAttribute) : property
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();
    }
}
