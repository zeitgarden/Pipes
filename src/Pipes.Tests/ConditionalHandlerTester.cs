using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace Pipes.Tests
{
    [TestFixture]
    public class ConditionalHandlerTester : InteractionContext<ConditionalHandler<string>>
    {
        private const string Message = "Foo";
        private bool _applies;
        protected override void beforeEach()
        {
            _applies = false;
            MockFor<ICondition<string>>()
                .Stub(x => x.Applies(Message))
                .Return(_applies).WhenCalled(x => x.ReturnValue = _applies);
        }

        [Test]
        public void if_condition_applies_inner_handler_is_executed()
        {
            _applies = true;
            ClassUnderTest.Handle(Message);
            MockFor<IHandler<string>>().AssertWasCalled(x => x.Handle(Message));
        }

        [Test]
        public void if_condition_does_not_applies_inner_handler_is_not_executed()
        {
            _applies = false;
            ClassUnderTest.Handle(Message);
            MockFor<IHandler<string>>().AssertWasNotCalled(x => x.Handle(Message));
        }
    }
}