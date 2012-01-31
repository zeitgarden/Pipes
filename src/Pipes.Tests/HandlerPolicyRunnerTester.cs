using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace Pipes.Tests
{
    [TestFixture]
    public class HandlerPolicyRunnerTester : InteractionContext<HandlerPolicyRunner>
    {
        private MessageGraph _graph;
        private IHandlerPolicy[] _policies;

        protected override void beforeEach()
        {
            _policies = Services.CreateMockArrayFor<IHandlerPolicy>(2);
            _graph = new MessageGraph();
            _graph.NewChainFor(typeof (LogoutMessage));
            _graph.NewChainFor(typeof (NewUserMessage));

            Services.Inject(_graph);

            ClassUnderTest.Activate(null, null);
        }

        [Test]
        public void each_policy_is_executed_against_the_graph_chains()
        {
            _graph
                .Chains
                .Each(chain => _policies.Each(policy => policy.AssertWasCalled(x => x.Configure(chain))));
        }
    }
}