using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;

namespace Pipes
{
    public class HandlerPolicyRunner : IActivator
    {
        private readonly IEnumerable<IHandlerPolicy> _policies;
        private readonly MessageGraph _graph;

        public HandlerPolicyRunner(IEnumerable<IHandlerPolicy> policies, MessageGraph graph)
        {
            _policies = policies;
            _graph = graph;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            _graph.Chains.Each(chain =>
            {
                foreach (var policy in _policies)
                {
                    policy.Configure(chain);
                }
            });
        }
    }
}