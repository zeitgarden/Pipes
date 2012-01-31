using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;
using Rhino.Mocks;

namespace Pipes.Tests
{
    [TestFixture]
    public class HandlerRegisterTester : InteractionContext<HandlerRegister>
    {
        private MessageGraph _graph;

        protected override void beforeEach()
        {
            _graph = new MessageGraph();
            var chain1 = _graph.NewChainFor(typeof(LogoutMessage));
            var chain2 = _graph.NewChainFor(typeof(NewUserMessage));
            chain1.Top = MockRepository.GenerateMock<MessageNode>();
            chain2.Top = MockRepository.GenerateMock<MessageNode>();
            chain1.Top.Stub(x => x.ToObjectDef()).Return(new HandlerNode(typeof(GenericHandler1<LogoutMessage>)).ToObjectDef());
            chain2.Top.Stub(x => x.ToObjectDef()).Return(new HandlerNode(typeof(GenericHandler1<NewUserMessage>)).ToObjectDef());
            Services.Inject(_graph);
            ClassUnderTest.Activate(null, null);
        }

        [Test]
        public void register_each_chain_def_against_the_injected_facility()
        {
            _graph
                .Chains
                .Each(chain => MockFor<IHandlerFacility>().AssertWasCalled(x => x.Register(chain.MessageType, chain.ToObjectDef())));
        }
    }
}