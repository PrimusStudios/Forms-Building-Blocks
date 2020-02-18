//using DryIoc;
//using System;
//using IContainer = Forms.BuildingBlocks.Interfaces.DI.IContainer;

//namespace Demo.Container
//{
//    public class DryIocContainer : IContainer
//    {
//        readonly DryIoc.IContainer container;

//        public DryIocContainer()
//        {
//            container = new DryIoc.Container();
//        }

//        public object Resolve(Type type)
//        {
//            return container.Resolve(type, IfUnresolved.Throw);
//        }

//        public T Resolve<T>()
//        {
//            return container.Resolve<T>();
//        }

//        public T Resolve<T>(string key)
//        {
//            return container.Resolve<T>(key);
//        }

//        public void Register<TInterface, TType>() where TType : TInterface
//        {
//            container.Register<TInterface, TType>();
//        }

//        public void RegisterSingleton<TInterface, TType>() where TType : TInterface
//        {
//            container.Register<TInterface, TType>(Reuse.Singleton);
//        }

//        public void Register(Type type)
//        {
//            container.Register(type, Reuse.Transient);
//        }

//        public void RegisterInstance(object instance)
//        {
//            container.UseInstance(instance);
//        }
//    }
//}
