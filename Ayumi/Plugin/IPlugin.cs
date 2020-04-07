using System;

namespace Ayumi.Plugin {
    public interface IPlugin {
        String ComponentName { get; }
        String ComponentDesc { get; }
        Object Process(Object processArgs);
    }
}
