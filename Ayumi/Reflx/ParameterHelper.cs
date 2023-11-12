using System.Linq;
using System.Reflection;

namespace Reflx {
    public class ParameterHelper : IParameterHelper {
        public TAttribute GetDecorator<TAttribute>(ParameterInfo parameter) =>
            parameter == null ? default(TAttribute) : parameter
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();
    }
}
