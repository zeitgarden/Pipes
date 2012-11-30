using System;
using FubuCore;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes
{
    public class HandlerEngine : IHandlerFactory, IHandlerFacility
    {
        private readonly IContainerFacility _containerFacility;
        private readonly IServiceLocator _serviceLocator;

        public HandlerEngine(IContainerFacility containerFacility, IServiceLocator serviceLocator)
        {
            _containerFacility = containerFacility;
            _serviceLocator = serviceLocator;
        }

        public IHandler<T> GetHandler<T>(Guid id)
        {
            return _serviceLocator.GetInstance<IHandler<T>>(id.ToString());
        }

        public void Register(Type messageType, ObjectDef def)
        {
            var handlerType = typeof(IHandler<>).MakeGenericType(messageType);
            _containerFacility.Register(handlerType, def);
        }
    }
}