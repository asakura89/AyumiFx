using System;

namespace WebLib.Web
{
    /// <summary>
    /// Object factory for ask of instance using it's abstraction.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// Get registered implementation using abstraction.
        /// </summary>
        /// <typeparam name="A">Abstract class / Interface</typeparam>
        /// <returns>Abstraction</returns>
        A GetRegisteredObject<A>();

        /// <summary>
        /// Get registered implementation using abstraction.
        /// </summary>
        /// <typeparam name="A">Abstract class / Interface</typeparam>
        /// <param name="constructorParamList">Implementation construtor parameter</param>
        /// <returns>Abstraction</returns>
        A GetRegisteredObject<A>(params Object[] constructorParamList);

        /// <summary>
        /// Register abstraction and implementation.
        /// </summary>
        /// <typeparam name="A">Abstract class / Interface</typeparam>
        /// <typeparam name="I">Implementation class</typeparam>
        void RegisterObject<A, I>();
    }
}