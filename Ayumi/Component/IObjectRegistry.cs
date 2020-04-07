using System;

namespace Ayumi.Component {
    public interface IObjectRegistry {
        A GetRegisteredObject<A>();
        A GetRegisteredObject<A>(params Object[] constructorParamList);
        void RegisterObject<A, I>() where I : A;
    }
}