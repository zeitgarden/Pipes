using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;

namespace Pipes.Tests
{
    [TestFixture]
    public class WrapWithHandlerAttributeConventionTester : InteractionContext<WrapWithHandlerAttributeConvention>
    {
        private MessageChain _chain1;
        private MessageChain _chain2;
        private MessageChain _chain3;

        protected override void beforeEach()
        {
            _chain1 = new MessageChain(typeof(NewUserMessage))
            {
                Top = new HandlerNode(typeof(Handler1))
            };
            _chain2 = new MessageChain(typeof(NewUserMessage))
            {
                Top = new HandlerNode(typeof(Handler2))
            };
            _chain3 = new MessageChain(typeof(NewUserMessage))
            {
                Top = new HandlerNode(typeof(Handler3))
            };

            ClassUnderTest.Configure(_chain1);
            ClassUnderTest.Configure(_chain2);
            ClassUnderTest.Configure(_chain3);
        }

        [Test]
        public void the_chain_handler_is_wrapped_with_wrapper_nodes()
        {
            _chain1
                .Top.ShouldNotBeNull().ShouldBeOfType<WrapperNode>()
                .Child.ShouldNotBeNull().ShouldBeOfType<WrapperNode>()
                .Child.ShouldNotBeNull().ShouldBeOfType<HandlerNode>()
                .HandlerType.ShouldEqual(typeof(Handler1));
        }

        [Test]
        public void wraps_the_chain_with_the_decorated_wrappers()
        {
            _chain1.OfType<WrapperNode>()
                .ShouldContain(x => x.WrapperType == typeof(Wrapper1<NewUserMessage>));
        }

        [Test]
        public void can_wrap_with_decorated_open_generic_wrappers()
        {
            _chain1.OfType<WrapperNode>()
                .ShouldContain(x => x.WrapperType == typeof(Wrapper2<NewUserMessage>));
        }

        [Test]
        public void if_the_decorated_wrapper_does_not_match_the_message_type_the_chain_is_not_modified()
        {
            _chain2.Top
                .ShouldNotBeNull().ShouldBeOfType<HandlerNode>()
                .HandlerType.ShouldEqual(typeof(Handler2));
            _chain2.Top.Child.ShouldBeNull();
        }

        [Test]
        public void if_the_handler_is_not_decorated_the_chain_is_not_modified()
        {
            _chain3.Top
                .ShouldNotBeNull().ShouldBeOfType<HandlerNode>()
                .HandlerType.ShouldEqual(typeof(Handler3));
            _chain3.Top.Child.ShouldBeNull();
        }


        public class Wrapper1<T> : IHandler<T>
        {
            private readonly IHandler<T> _inner;

            public Wrapper1(IHandler<T> inner )
            {
                _inner = inner;
            }

            public void Handle(T message)
            {
                
            }
        }
        public class Wrapper2<T> : IHandler<T>
        {
            private readonly IHandler<T> _inner;

            public Wrapper2(IHandler<T> inner)
            {
                _inner = inner;
            }
            public void Handle(T message)
            {
                
            }
        }

        [WrapWithHandler(typeof(Wrapper1<NewUserMessage>))]
        [WrapWithHandler(typeof(Wrapper2<>))]
        public class Handler1 : IHandler<NewUserMessage>
        {
            public void Handle(NewUserMessage message)
            {
            }
        }

        [WrapWithHandler(typeof(Wrapper1<LogoutMessage>))]
        public class Handler2 : IHandler<NewUserMessage>
        {
            public void Handle(NewUserMessage message)
            {
            }
        }

        public class Handler3 : IHandler<NewUserMessage>
        {
            public void Handle(NewUserMessage message)
            {
            }
        }
    }
}