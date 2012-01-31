using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;

namespace Pipes
{
    public class HandlerRegister : IActivator
    {
        private readonly MessageGraph _graph;
        private readonly IHandlerFacility _facility;

        public HandlerRegister(MessageGraph graph, IHandlerFacility facility)
        {
            _graph = graph;
            _facility = facility;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            _graph
                .Chains
                .Each(chain => _facility.Register(chain.MessageType, chain.ToObjectDef()));
        }
    }
}