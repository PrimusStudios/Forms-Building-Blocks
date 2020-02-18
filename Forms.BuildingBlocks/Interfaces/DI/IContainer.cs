using System;

namespace Forms.BuildingBlocks.Interfaces.DI
{
    public interface IContainer
    {
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

        IServiceProvider Create();
    }
}