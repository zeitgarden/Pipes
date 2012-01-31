using System;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.StructureMap;
using StructureMap;

namespace Pipes
{
    public class HandlerEngine : IHandlerFactory, IHandlerFacility
    {
        private readonly IContainer _container;

        public HandlerEngine(IContainer container)
        {
            _container = container;
        }

        public IHandler<T> GetHandler<T>(Guid id)
        {
            return _container.GetInstance<IHandler<T>>(id.ToString());
        }

        public void Register(Type messageType, ObjectDef def)
        {
            var handlerType = typeof(IHandler<>).MakeGenericType(messageType);
            _container.Configure(x => x.For(handlerType).Add(new ObjectDefInstance(def)));
        }
    }
}