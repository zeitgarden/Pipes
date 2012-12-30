using System;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes
{
    public class HandlerFacility : IHandlerFacility
    {
        private readonly IContainerFacility _containerFacility;

        public HandlerFacility(IContainerFacility containerFacility)
        {
            _containerFacility = containerFacility;
        }

        public void Register(Type messageType, ObjectDef def)
        {
            var handlerType = typeof(IHandler<>).MakeGenericType(messageType);
            _containerFacility.Register(handlerType, def);
        }
    }
}