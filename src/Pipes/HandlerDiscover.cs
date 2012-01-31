using System;
using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using Pipes.Nodes;

namespace Pipes
{
    public class HandlerDiscover : IActivator
    {
        private readonly MessageGraph _graph;
        private readonly IEnumerable<IHandlerSource> _sources;

        public HandlerDiscover(IEnumerable<IHandlerSource> sources, MessageGraph graph)
        {
            _sources = sources;
            _graph = graph;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var types = _sources.SelectMany(x => x.GetHandlers()).Distinct();
            types.Each(setHandlerNode);
        }

        private void setHandlerNode(Type handlerType)
        {
            var message = handlerType.FindParameterTypeTo(typeof (IHandler<>));
            _graph.NewChainFor(message).Top = new HandlerNode(handlerType);
        }
    }
}