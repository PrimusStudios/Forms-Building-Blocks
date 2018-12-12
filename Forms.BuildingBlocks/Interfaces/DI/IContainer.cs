using System;

namespace Forms.BuildingBlocks.Interfaces.DI
{
    public interface IContainer
    {
        /// <summary>
        ///     Resolves object by type.
        /// </summary>
        object Resolve(Type type);

        /// <summary>
        ///     Resolves Registered Type.
        /// </summary>
        T Resolve<T>();

        /// <summary>
        ///     Resolves Registered Type by string Key.
        /// </summary>
        T Resolve<T>(string key);

        /// <summary>
        ///     Registers a type by interface.
        /// </summary>
        void Register<TInterface, TType>() where TType : TInterface;

        /// <summary>
        ///     Registers a Singleton of type by interface.
        /// </summary>
        void RegisterSingleton<TInterface, TType>() where TType : TInterface;

        /// <summary>
        ///     Registers a type by its type
        /// </summary>
        void Register(Type type);

        /// <summary>
        ///     Registers an Instance of an object.
        /// </summary>
        void RegisterInstance(object instance);
    }
}