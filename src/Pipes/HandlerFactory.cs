using System;
using FubuCore;

namespace Pipes
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public HandlerFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IHandler<T> GetHandler<T>(Guid id)
        {
            return _serviceLocator.GetInstance<IHandler<T>>(id.ToString());
        }
    }
}