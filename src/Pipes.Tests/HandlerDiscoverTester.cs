using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;
using Rhino.Mocks;

namespace Pipes.Tests
{
    [TestFixture]
    public class HandlerDiscoverTester : InteractionContext<HandlerDiscover>
    {
        private MessageGraph _graph;
        private IHandlerSource[] _sources;

        protected override void beforeEach()
        {
            _graph = new MessageGraph();
            _sources = Services.CreateMockArrayFor<IHandlerSource>(3);

            _sources[0].Stub(x => x.GetHandlers()).Return(new[] { typeof(GenericHandler1<NewUserMessage>), typeof(GenericHandler1<NewUserMessage>) });
            _sources[1].Stub(x => x.GetHandlers()).Return(new[] { typeof(GenericHandler1<NewUserMessage>), typeof(GenericHandler1<LogoutMessage>) });
            _sources[2].Stub(x => x.GetHandlers()).Return(new[] { typeof(GenericHandler2<NewUserMessage>), typeof(GenericHandler2<LogoutMessage>) });

            Services.Inject(_graph);
            ClassUnderTest.Activate(null, null);
        }

        [Test]
        public void gets_the_handler_types_from_the_injected_sources()
        {
            _sources.Each(source => RhinoMocksExtensions.AssertWasCalled<IHandlerSource>(source, x => x.GetHandlers()));
        }

        [Test]
        public void generates_a_chain_per_unique_handler()
        {
            _graph.Chains.ShouldHaveCount(4);
        }

        [Test]
        public void generates_a_chain_per_handler_message_type()
        {
            new[] {typeof (NewUserMessage), typeof (LogoutMessage)}
                .Each(message => _graph.ChainsOf(message).ShouldHaveCount(2));
        }

        [Test]
        public void each_chain_top_node_is_a_handler_node_instance()
        {
            _graph.Chains.Each(x => x.Top.ShouldBeOfType<HandlerNode>());
        }

        [Test]
        public void each_handler_node_targets_a_handler_type_retrieved_from_the_handler_sources()
        {
            var nodes = _graph.Chains.Select(x => x.Top).Cast<HandlerNode>().ToList();
            new[]
                {
                    typeof (GenericHandler1<NewUserMessage>), typeof (GenericHandler2<NewUserMessage>),
                    typeof (GenericHandler1<LogoutMessage>), typeof (GenericHandler2<LogoutMessage>)
                }.Each(handler => nodes.ShouldContain(node => node.HandlerType == handler));
        }
    }
}