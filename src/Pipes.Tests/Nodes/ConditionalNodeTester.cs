using FubuMVC.Core.Registration.ObjectGraph;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;

namespace Pipes.Tests.Nodes
{
    [TestFixture]
    public class open_generic_conditional_node : InteractionContext<ConditionalNode>
    {
        private ObjectDef _objectDef;

        protected override void beforeEach()
        {
            Services.Inject(new ConditionalNode(typeof (Never<>)));
            ClassUnderTest.Chain = new MessageChain(typeof (string));
            _objectDef = ClassUnderTest.ToObjectDef();
        }

        [Test]
        public void condition_type_is_open_generic()
        {
            ClassUnderTest.ConditionType.ShouldEqual(typeof (Never<>));
        }

        [Test]
        public void object_def_type_closes_conditional_handler_with_message_type()
        {
            _objectDef.Type.ShouldEqual(typeof (ConditionalHandler<string>));
        }

        [Test]
        public void object_def_closes_the_condition_with_message_type_and_sets_it_as_dependency()
        {
            _objectDef.FindDependencyDefinitionFor<ICondition<string>>()
                .ShouldNotBeNull()
                .Type.ShouldEqual(typeof (Never<string>));
        }
    }

    [TestFixture]
    public class closed_generic_conditional_node : InteractionContext<ConditionalNode>
    {
        private ObjectDef _objectDef;

        protected override void beforeEach()
        {
            Services.Inject(new ConditionalNode(typeof(Never<string>)));
            ClassUnderTest.Chain = new MessageChain(typeof(string));
            _objectDef = ClassUnderTest.ToObjectDef();
        }

        [Test]
        public void condition_type_is_closed_generic()
        {
            ClassUnderTest.ConditionType.ShouldEqual(typeof(Never<string>));
        }

        [Test]
        public void object_def_type_closes_conditional_handler_with_message_type()
        {
            _objectDef.Type.ShouldEqual(typeof(ConditionalHandler<string>));
        }

        [Test]
        public void object_def_uses_the_condition_type_and_sets_it_as_dependency()
        {
            _objectDef.FindDependencyDefinitionFor<ICondition<string>>()
                .ShouldNotBeNull()
                .Type.ShouldEqual(typeof(Never<string>));
        }
    }
}