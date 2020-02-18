using Autofac;
using System;

namespace Demo.Containers
{
    public class AutofacContainer : Forms.BuildingBlocks.Interfaces.DI.IContainer, IServiceProvider
    {
        ContainerBuilder Builder;
        Autofac.IContainer Container;

        public AutofacContainer(ContainerBuilder builder)
        {
            Builder = builder;
        }
        public IServiceProvider Create()
        {
            Container = Builder.Build();
            return this;
        }

        public object GetService(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        public void Register<TInterface, TType>() where TType : TInterface
        {
            Builder.RegisterType<TType>().As<TInterface>();
        }

        public void Register(Type type)
        {
            Builder.RegisterType(type);
        }

        public void RegisterInstance(Type type, object instance)
        {
            Builder.RegisterInstance(instance).As(type);
        }


        public void RegisterSingleton<TInterface, TType>() where TType : TInterface
        {
            Builder.RegisterType<TType>().As<TInterface>().SingleInstance();
        }
    }
}
