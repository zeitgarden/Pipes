using System;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.StructureMap;
using StructureMap;

namespace Pipes.StructureMap
{
    public class StructureMapHandlerEngine : IHandlerFacility, IHandlerFactory
    {
        private readonly IContainer _container;

        public StructureMapHandlerEngine(IContainer container)
        {
            _container = container;
        }

        public void Register(Type messageType, ObjectDef def)
        {
            var handlerType = typeof(IHandler<>).MakeGenericType(messageType);
            _container.Configure(x => x.For(handlerType).Use(new ObjectDefInstance(def)));
        }

        public IHandler<T> GetHandler<T>(Guid id)
        {
            return _container.GetInstance<IHandler<T>>(id.ToString());
        }
    }
}