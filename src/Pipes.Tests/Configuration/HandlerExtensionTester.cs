using System.Linq;
using Bottles;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Configuration;

namespace Pipes.Tests.Configuration
{
    [TestFixture]
    public class HandlerExtensionTester : InteractionContext<HandlerExtension>
    {
        private ServiceGraph _services;

        protected override void beforeEach()
        {
            var registry = new FubuRegistry();
            ClassUnderTest.Configure(registry);
            _services = registry.BuildGraph().Services;
        }

        [Test]
        public void message_graph_service()
        {
            _services.ServicesFor<MessageGraph>()
                .ShouldHaveCount(1)
                .First().Value.ShouldNotBeNull();
        }

        [Test]
        public void handlers_engine_is_set_as_handler_facility()
        {
            _services.ServicesFor<IHandlerFacility>()
                .ShouldHaveCount(1)
                .First().Type.ShouldEqual(typeof (HandlerEngine));
        }

        [Test]
        public void handlers_engine_is_set_as_handler_factory()
        {
            _services.ServicesFor<IHandlerFactory>()
                .ShouldHaveCount(1)
                .First().Type.ShouldEqual(typeof(HandlerEngine));
        }

        [Test]
        public void publisher_service()
        {
            _services.ServicesFor<IPublisher>()
                .ShouldHaveCount(1)
                .First().Type.ShouldEqual(typeof(Publisher));
        }

        [Test]
        public void wrap_with_conditional_attribute_convention_policy_is_added_to_the_handler_policy_service()
        {
            _services.ServicesFor<IHandlerPolicy>()
                .ShouldContain(x => x.Type == typeof(WrapWithConditionalAttributeConvention));
        }

        [Test]
        public void wrap_with_handler_attribute_convention_policy_is_added_to_the_handler_policy_service()
        {
            _services.ServicesFor<IHandlerPolicy>()
                .ShouldContain(x => x.Type == typeof(WrapWithHandlerAttributeConvention));
        }

        [Test]
        public void handler_discover_is_added_to_the_activator_service()
        {
            _services.ServicesFor<IActivator>()
                .ShouldContain(x => x.Type == typeof(HandlerDiscover));
        }

        [Test]
        public void handler_policy_runner_is_added_to_the_activator_service()
        {
            _services.ServicesFor<IActivator>()
                .ShouldContain(x => x.Type == typeof(HandlerPolicyRunner));
        }


        [Test]
        public void handler_register_is_added_to_the_activator_service()
        {
            _services.ServicesFor<IActivator>()
                .ShouldContain(x => x.Type == typeof(HandlerRegister));
        }
    }
}