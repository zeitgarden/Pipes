using System.Linq;
using Bottles;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.StructureMap;

namespace Pipes.Tests.Configuration.StructureMap
{
    [TestFixture]
    public class MessagesRegistryTester : InteractionContext<MessagesRegistry>
    {
        protected override void beforeEach()
        {
            Container.Configure(x => x.AddRegistry(ClassUnderTest));
        }

        [Test]
        public void message_graph_service()
        {
            Container.GetAllInstances<MessageGraph>()
                .ShouldHaveCount(1)
                .First().ShouldEqual(Container.GetInstance<MessageGraph>());
        }

        [Test]
        public void handlers_engine_is_set_as_handler_facility()
        {
            Container.GetAllInstances<IHandlerFacility>()
                .ShouldHaveCount(1)
                .First().GetType().ShouldEqual(typeof(HandlerEngine));
        }

        [Test]
        public void handlers_engine_is_set_as_handler_factory()
        {
            Container.GetAllInstances<IHandlerFactory>()
                .ShouldHaveCount(1)
                .First().GetType().ShouldEqual(typeof (HandlerEngine));
        }

        [Test]
        public void publisher_service()
        {
            Container.GetAllInstances<IPublisher>()
              .ShouldHaveCount(1)
              .First().GetType().ShouldEqual(typeof(Publisher));
        }

        [Test]
        public void wrap_with_conditional_attribute_convention_policy_is_added_to_the_handler_policy_service()
        {
            Container.GetAllInstances<IHandlerPolicy>()
                .OfType<WrapWithConditionalAttributeConvention>()
                .ShouldHaveCount(1);
        }

        [Test]
        public void wrap_with_handler_attribute_convention_policy_is_added_to_the_handler_policy_service()
        {
            Container.GetAllInstances<IHandlerPolicy>()
                .OfType<WrapWithHandlerAttributeConvention>()
                .ShouldHaveCount(1);
        }

        [Test]
        public void handler_discover_is_added_to_the_activator_service()
        {
            Container.GetAllInstances<IActivator>()
               .OfType<HandlerDiscover>()
               .ShouldHaveCount(1);
        }

        [Test]
        public void handler_policy_runner_is_added_to_the_activator_service()
        {
            Container.GetAllInstances<IActivator>()
               .OfType<HandlerPolicyRunner>()
               .ShouldHaveCount(1);
        }


        [Test]
        public void handler_register_is_added_to_the_activator_service()
        {
            Container.GetAllInstances<IActivator>()
               .OfType<HandlerRegister>()
               .ShouldHaveCount(1);
        }
    }
}