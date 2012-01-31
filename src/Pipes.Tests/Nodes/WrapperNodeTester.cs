using FubuMVC.Core.Registration.ObjectGraph;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;

namespace Pipes.Tests.Nodes
{
    [TestFixture]
    public class open_generic_wrapper_node : InteractionContext<WrapperNode>
    {
        private ObjectDef _def;
        protected override void beforeEach()
        {
            Services.Inject(new WrapperNode(typeof(GenericHandler1<>))
            {
                Chain = new MessageChain(typeof(LogoutMessage))
            });
            _def = ClassUnderTest.ToObjectDef();
        }

        [Test]
        public void wrapper_type_is_open_generic()
        {
            ClassUnderTest.WrapperType.ShouldEqual(typeof(GenericHandler1<>));
        }

        [Test]
        public void object_def_type_closes_to_chain_message_type()
        {
            _def.Type.ShouldEqual(typeof(GenericHandler1<LogoutMessage>));
        }
    }

    [TestFixture]
    public class closed_generic_wrapper_node : InteractionContext<WrapperNode>
    {
        private ObjectDef _def;
        protected override void beforeEach()
        {
            Services.Inject(new WrapperNode(typeof(GenericHandler1<NewUserMessage>))
            {
                Chain = new MessageChain(typeof(NewUserMessage))
            });
            _def = ClassUnderTest.ToObjectDef();
        }

        [Test]
        public void wrapper_type_is_closed_generic()
        {
            ClassUnderTest.WrapperType.ShouldEqual(typeof(GenericHandler1<NewUserMessage>));
        }

        [Test]
        public void object_def_type_is_wrapper_type()
        {
            _def.Type.ShouldEqual(typeof(GenericHandler1<NewUserMessage>));
        }
    }
}