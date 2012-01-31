using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace Pipes.Tests
{
    [TestFixture]
    public class PublisherTester : InteractionContext<Publisher>
    {
        private IHandlerFactory _factory;
        private MessageGraph _graph;
        private IList<IHandler<NewUserMessage>> _handlers;
        private NewUserMessage _message;
        protected override void beforeEach()
        {
            _factory = MockFor<IHandlerFactory>();
            _graph = new MessageGraph();
            _message = new NewUserMessage();
            _handlers = new List<IHandler<NewUserMessage>>();

            Enumerable.Range(1, 4)
                .Select(x => typeof (NewUserMessage))
                .Select(_graph.NewChainFor)
                .Each(chain =>
                          {
                              var handler = MockRepository.GenerateMock<IHandler<NewUserMessage>>();
                              _factory
                                  .Expect(x => x.GetHandler<NewUserMessage>(chain.Id))
                                  .Return(handler);
                              _handlers.Add(handler);
                          });

            Services.Inject(_graph);
            ClassUnderTest.Publish(_message);
        }

        [Test]
        public void the_handlers_are_returned_from_the_factory()
        {
            _factory.VerifyAllExpectations();
        }

        [Test]
        public void all_the_handlers_handle_the_published_message()
        {
            _handlers.Each(handler => handler.AssertWasCalled(x => x.Handle(_message)));
        }

    }
}