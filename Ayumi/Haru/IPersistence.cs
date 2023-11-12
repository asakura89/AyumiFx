using System;
using System.Collections.Generic;

namespace Haru {
    public interface IPersistence {
        IEnumerable<String> Keys { get; }
        Boolean Exists(String key);
        String Get(String key);
        void Set(String key, String value);
        void Remove(String key);
        void Clear();
    }

    public interface IAppSession : IPersistence {
        new Object Get(String key);
        void Set(String key, Object value);
    }

    public interface IAppCookie : IPersistence {

    }
}
