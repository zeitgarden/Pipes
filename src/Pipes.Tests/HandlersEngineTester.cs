using System;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuTestingSupport;
using NUnit.Framework;

namespace Pipes.Tests
{
    [TestFixture]
    public class HandlersEngineTester : InteractionContext<HandlerEngine>
    {
        private IHandler<NewUserMessage> _handler;
        private Guid _handlerId;
        private ObjectDef _handlerDef;

        protected override void beforeEach()
        {
            _handler = MockFor<IHandler<NewUserMessage>>();
            _handlerId = Guid.NewGuid();
            _handlerDef = ObjectDef.ForType<GenericHandler1<LogoutMessage>>();
            Container.Configure(x => x.For<IHandler<NewUserMessage>>().Add(_handler).Named(_handlerId.ToString()));
            ClassUnderTest.Register(typeof(LogoutMessage), _handlerDef);
        }

        [Test]
        public void get_handler_returns_the_instance_from_the_container_using_the_id_as_the_name()
        {
            ClassUnderTest.GetHandler<NewUserMessage>(_handlerId).ShouldEqual(_handler);
        }

        [Test]
        public void register_adds_the_handler_def_type_to_the_container_by_message_type()
        {
            Container.GetInstance<IHandler<LogoutMessage>>(_handlerDef.Name)
                .ShouldBeOfType<GenericHandler1<LogoutMessage>>();
        }
    }


}